namespace Ubique.Models.ViewModels
{
	public class SubCategoryVM
	{
		public required SubCategory SubCategory { get; set; }
		public required List<Category> Categories { get; set; }
	}
}
