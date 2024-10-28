using DoAnWeb2024.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb2024.Controllers
{
	public class BlogController : Controller
	{
		
		public IActionResult Index()
		{
			return View();
		}
	}
}
