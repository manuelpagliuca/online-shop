using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
				includeProperties: "Product")
			};

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				double price = GetPriceBasedOnQuantity(cart); // if the price change given the item count
				ShoppingCartVM.OrderTotal += (price * cart.Count);
			}

			return View(ShoppingCartVM);
		}


		private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			return shoppingCart.Product.Price;
			// TODO: maybe could be needed in the future, give different price values
			//if (shoppingCart.Count <= 50)
			//{
			//	return shoppingCart.Product.Price;
			//}
			//else
			//{
			//	if (shoppingCart.Count <= 100)
			//	{
			//		return shoppingCart.Product.Price50;
			//	}
			//	else
			//	{
			//		return shoppingCart.Product.Price100;
			//	}
			//}
		}
	}
}
