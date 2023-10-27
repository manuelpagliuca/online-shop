using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
			ShoppingCart cart = new()
			{
				Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "SubCategory.Category"),
				Count = 1,
				ProductId = productId
			};

			return View(cart);
		}

		[HttpPost]
		[Authorize]
		public IActionResult Details(ShoppingCart shoppingCart)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			shoppingCart.ApplicationUserId = userId;

			ShoppingCart cartFromDb =
				_unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

			// avoid shopping cart duplication
			if (cartFromDb != null)
			{
				// shopping cart exists
				cartFromDb.Count += shoppingCart.Count;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
			}
			else
			{
				// add cart record
				_unitOfWork.ShoppingCart.Add(shoppingCart);
			}

			_unitOfWork.Save();

			Product product = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId, includeProperties: "SubCategory.Category");
			string productCategoryFilter = product.SubCategory.Category.Name.Split(" ")[0];

			return RedirectToAction("Index", new { categoryFilter = productCategoryFilter });
		}
	}
}
