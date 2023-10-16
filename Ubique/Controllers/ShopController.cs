using Microsoft.AspNetCore.Mvc;

namespace Ubique.Controllers
{
	public class ShopController : Controller
	{
		public IActionResult Index(string category)
		{
			return View(category);
		}
	}
}
