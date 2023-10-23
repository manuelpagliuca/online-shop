using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubique.Models.ViewModels
{
	public class ProductVM
	{
		public Product Product { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> CategoryList { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> SubCategoryList { get; set; } // Pre-Loading SubCategories List in `Edit`
	}
}
