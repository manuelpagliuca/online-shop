using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class ShopController : Controller
	{
		private readonly ILogger<ShopController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public ShopController(ILogger<ShopController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index(string categoryFilter)
		{
			if (categoryFilter == null)
			{
				return View("Page404");
			}

			IEnumerable<Product> productList =
				_unitOfWork.Product.GetAll(includeProperties: "SubCategory.Category")
				.Where(u => u.SubCategory.Category.Name.Contains(categoryFilter));

			return View(productList);
		}

		public IActionResult Details(int productId)
		{
			Product product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "SubCategory.Category");

			return View(product);
		}
	}
}
