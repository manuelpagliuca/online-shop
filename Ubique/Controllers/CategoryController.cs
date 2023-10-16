using Microsoft.AspNetCore.Mvc;

namespace Ubique.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
