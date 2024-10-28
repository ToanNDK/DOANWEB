using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoAnWeb2024.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext context)
		{
			
			_dataContext = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString(); //111
				var orderItem = new OrderModel();
				orderItem.OrderCode=ordercode;
				orderItem.UserName=userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartitems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					var product = (from p in _dataContext.Products
								  where p.Id == cart.ProductId
								  select p).FirstOrDefault();
					orderdetails.Product = product;
					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Thanh toán thành công, vui lòng chờ";
				return RedirectToAction("Index","Cart");
			}
			return View();
		}
	}
}
