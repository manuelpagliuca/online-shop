using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class FaucetController : Controller
	{
		private readonly ILogger<FaucetController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public FaucetController(ILogger<FaucetController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> productList =
				_unitOfWork.Product.GetAll(includeProperties: "SubCategory.Category")
				.Where(u => u.SubCategory.Category.Name.Contains("Rubinetteria"));

			return View(productList);
		}

		public IActionResult Details(int productId)
		{
			Product product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "SubCategory.Category");

			return View(product);
		}
	}
}
