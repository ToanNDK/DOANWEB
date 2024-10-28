using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using DoAnWeb2024.Repository.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {

        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderBy(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id); // tìm dữ liệu cần sửa dựa vào ID

            return View(brand);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {


            if (ModelState.IsValid)
            {

                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == brand.Slug); // ktra có trùng slug k => sp có trong db chưa
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã tồn tại ");
                    return View(brand);
                }



                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công !";
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
                /*return BadRequest(errorMessage);*/
            }


            return View(brand);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thương hiệu đã được xóa !";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {


            if (ModelState.IsValid)
            {

                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == brand.Slug); // ktra có trùng slug k => sp có trong db chưa
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã tồn tại ");
                    return View(brand);
                }



                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thương hiệu thành công !";
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
                /*return BadRequest(errorMessage);*/
            }


            return View(brand);
        }

    }
}
