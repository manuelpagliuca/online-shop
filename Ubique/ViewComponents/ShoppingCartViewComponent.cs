using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Utility;

namespace Ubique.ViewComponents
{
	public class ShoppingCartViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _unitOfWork;

		public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim != null)
			{
				if (HttpContext.Session.GetInt32(StaticDetails.SessionCart) == null)
				{
					var cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList();

					int numberOfItems = 0;

					foreach (var item in cartItems)
					{
						numberOfItems += cartItems.Count;
					}

					HttpContext.Session.SetInt32(StaticDetails.SessionCart, numberOfItems);
				}

				return View(HttpContext.Session.GetInt32(StaticDetails.SessionCart));
			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}
		}
	}
}
