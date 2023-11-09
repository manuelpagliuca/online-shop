using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ubique.Models.ViewModels
{
	public class SubCategoryVM
	{
		public required SubCategory SubCategory { get; set; }
		[ValidateNever]
		public required List<Category> Categories { get; set; }
	}
}
