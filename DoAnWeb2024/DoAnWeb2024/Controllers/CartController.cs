using DoAnWeb2024.Models;
using DoAnWeb2024.Models.ViewModels;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb2024.Controllers
{
    public class CartController :Controller
    {
        private readonly DataContext _dataContext;
        public CartController (DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            List<CartItemModel> cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartitems,
                GrandTotal = cartitems.Sum(x => x.Quantity * x.Price),
            };
            return View(cartVM);
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
        public async Task <IActionResult> Add (int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);//lấy sản phảm theo Id
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItems == null) {
                cart.Add(new CartItemModel(product));
            }
            else // nếu đã có sp thì sẽ tăng sl 
            {
                cartItems.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart); // tạo 1 bộ nhớ tạm thời lưu trữ

			TempData["success"] = "Thêm vào giỏ hàng thành công!"; // lấy từ NotificationPartial
            return Redirect(Request.Headers["Referer"].ToString()); // chuyển về trang trước
        }
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			else // nếu đã có sp thì sẽ tăng sl 
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
            TempData["success"] = "Tăng số lượng thành công!"; // lấy từ NotificationPartial

            return RedirectToAction("Index"); // chuyển về trang trước
		}
		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
                --cartItem.Quantity;
			}
			else // nếu đã có sp thì sẽ tăng sl 
			{
                cart.RemoveAll(p => p.ProductId == Id);
			}
            if (cart.Count == 0) 
            {
				HttpContext.Session.Remove("Cart"); 

			}
            else
            {
				HttpContext.Session.SetJson("Cart", cart);

			}
            TempData["success"] = "Giảm số lượng thành công!"; // lấy từ NotificationPartial

            return RedirectToAction("Index"); // chuyển về trang trước
		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			cart.RemoveAll(p => p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart",cart);
			}
            TempData["success"] = "Xóa số lượng thành công"; // lấy từ NotificationPartial

            return RedirectToAction("Index");
		}
		public async Task<IActionResult> Clear ()
		{
			HttpContext.Session.Remove("Cart");
            TempData["success"] = "Xóa sản phẩm thành công!"; // lấy từ NotificationPartial

            return RedirectToAction("Index");
		}

	}
}
