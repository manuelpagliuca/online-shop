using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Utility;

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

		public IActionResult Index(string? categoryFilter)
		{
			if (string.IsNullOrEmpty(categoryFilter))
			{
				return View("Page404");
			}

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim != null)
			{
				List<ShoppingCart>? carts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList();
				int itemsCount = 0;

				foreach (ShoppingCart? itemCart in carts)
				{
					itemsCount += itemCart.Count;
				}

				HttpContext.Session.SetInt32(StaticDetails.SessionCart, itemsCount);
			}

			IEnumerable<Product> productList =
				_unitOfWork.Product
					.GetAll(includeProperties: "SubCategory,SubCategory.Category,ProductImages")
					.Where(u => u.SubCategory.Category.Name.Contains(categoryFilter));

			return View(productList);
		}

		public IActionResult Details(int productId)
		{
			ShoppingCart cart = new()
			{
				Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "SubCategory,SubCategory.Category,ProductImages"),
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

			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

			// avoid shopping cart duplication
			if (cartFromDb != null)
			{
				//shopping cart exists
				cartFromDb.Count += shoppingCart.Count;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
				_unitOfWork.Save();
			}
			else
			{
				// new shopping cart record
				_unitOfWork.ShoppingCart.Add(shoppingCart);
				_unitOfWork.Save();
			}

			int newCartCount = HttpContext.Session.GetInt32(StaticDetails.SessionCart) + shoppingCart.Count ?? 0;
			HttpContext.Session.SetInt32(StaticDetails.SessionCart, newCartCount);
			TempData["success"] = "Carrello aggiornato.";
			Product product = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId, includeProperties: "SubCategory.Category");
			string productCategoryFilter = product.SubCategory.Category.Name.Split(" ")[0];

			return RedirectToAction("Index", new { categoryFilter = productCategoryFilter });
		}
	}
}
