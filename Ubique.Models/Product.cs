using System.ComponentModel.DataAnnotations;

namespace Ubique.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		public string Brand { get; set; }

		[Required]
		[Display(Name = "Prezzo Lista")]
		[Range(1, 2000)]
		public double ListPrice { get; set; }

		[Required]
		[Display(Name = "Prezzo per 1-50")]
		[Range(1, 2000)]
		public double Price { get; set; }
	}
}
