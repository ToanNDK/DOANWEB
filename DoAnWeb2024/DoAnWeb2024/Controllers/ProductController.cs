using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb2024.Controllers
{
    public class ProductController :Controller
    {
        private readonly DataContext _dataContext;
        public ProductController (DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            return View(productsById);
        }
		public async Task<IActionResult> Search(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return View(new List<ProductModel>());
			}

			var products = await _dataContext.Products
				.Where(p => p.Name.Contains(query) || p.Description.Contains(query))
				.ToListAsync();

			return View(products);
		}

	}
}
