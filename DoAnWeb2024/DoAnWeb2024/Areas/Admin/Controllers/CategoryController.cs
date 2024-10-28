using DoAnWeb2024.Models;
using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb2024.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		
		public CategoryController(DataContext context)
		{
			_dataContext = context;
			
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id); // tìm dữ liệu cần sửa dựa vào ID

            return View(category);
        }
       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            

            if (ModelState.IsValid)
            {

                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug); // ktra có trùng slug k => sp có trong db chưa
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã tồn tại ");
                    return View(category);
                }
                


                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công !";
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


            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {


            if (ModelState.IsValid)
            {

                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug); // ktra có trùng slug k => sp có trong db chưa
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã tồn tại ");
                    return View(category);
                }



                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật danh mục thành công !";
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


            return View(category);
        }


        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Danh muc đã được xóa !";
            return RedirectToAction("Index");
        }
    }
}
