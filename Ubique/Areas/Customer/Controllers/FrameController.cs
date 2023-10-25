using Microsoft.AspNetCore.Mvc;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class FrameController : Controller
	{
		private readonly ILogger<FrameController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public FrameController(ILogger<FrameController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> productList =
				_unitOfWork.Product.GetAll(includeProperties: "SubCategory.Category")
				.Where(u => u.SubCategory.Category.Name.Contains("Serramenti"));

			return View(productList);
		}

		public IActionResult Details(int productId)
		{
			Product product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "SubCategory.Category");

			return View(product);
		}
	}
}
