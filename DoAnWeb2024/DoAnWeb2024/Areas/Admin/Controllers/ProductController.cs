using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnWeb2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 6; 
            int pageNumber = (page ?? 1); 

            var products = await _dataContext.Products
                .OrderBy(p => p.Id)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Create(ProductModel products)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", products.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", products.BrandId);

            if (ModelState.IsValid)
            {
                products.Slug = products.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == products.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                    return View(products);
                }

                if (products.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + products.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await products.ImageUpload.CopyToAsync(fs);
                    }

                    products.Image = imageName;
                }

                _dataContext.Add(products);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                // return BadRequest(errorMessage);
            }

            return View(products);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductModel products)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", products.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", products.BrandId);

            if (ModelState.IsValid)
            {
                products.Slug = products.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == products.Slug && p.Id != Id);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                    return View(products);
                }

                if (products.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + products.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await products.ImageUpload.CopyToAsync(fs);
                    }

                    products.Image = imageName;
                }

                _dataContext.Update(products);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                // return BadRequest(errorMessage);
            }

            return View(products);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (!string.Equals(product.Image, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldfileImage = Path.Combine(uploadDir, product.Image);
                if (System.IO.File.Exists(oldfileImage))
                {
                    System.IO.File.Delete(oldfileImage);
                }
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã được xóa!";
            return RedirectToAction("Index");
        }
    }
}
