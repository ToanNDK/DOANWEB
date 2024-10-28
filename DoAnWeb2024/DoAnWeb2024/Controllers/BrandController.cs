using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb2024.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController (DataContext context)
		{
			_dataContext = context;
			
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null) RedirectToAction("Index");

			var ProductByCategory = _dataContext.Products.Where(p => p.BrandId == brand.Id);
			return View(await ProductByCategory.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
