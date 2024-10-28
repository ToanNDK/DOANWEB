using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb2024.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController (DataContext context)
		{
			_dataContext = context;
		}
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode==ordercode).ToListAsync();
            return View(DetailsOrder);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            var order = await _dataContext.Orders.FindAsync(Id);

            if (order == null)
            {
                TempData["error"] = "Đơn hàng không tồn tại!";
                return RedirectToAction("Index");
            }

            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đơn hàng đã được xóa!";
            return RedirectToAction("Index");
        }


    }
}
