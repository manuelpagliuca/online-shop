using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ubique.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Il campo \"Nome\" è obbligatorio.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Il campo \"Description\" è obbligatorio.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Il campo \"Brand\" è obbligatorio.")]
		public string Brand { get; set; }

		[Display(Name = "Prezzo Lista")]
		[Range(1, 2000)]
		public double ListPrice { get; set; }

		[Display(Name = "Prezzo per 1-50")]
		[Range(1, 2000)]
		public double Price { get; set; }

		[Required(ErrorMessage = "Il campo \"Sotto Categoria\" è obbligatorio.")]
		[Display(Name = "Sotto Categoria")]
		public int SubCategoryId { get; set; }

		[ForeignKey("SubCategoryId")]
		public SubCategory SubCategory { get; set; }

		[ValidateNever]
		public List<ProductImage> ProductImages { get; set; }

		public bool IsValid()
		{
			if (string.IsNullOrEmpty(Name)) return false;
			if (string.IsNullOrEmpty(Description)) return false;
			if (string.IsNullOrEmpty(Brand)) return false;
			if (SubCategoryId == null) return false;

			return true;
		}
	}
}
