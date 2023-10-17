using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ubique.Models
{
	public class SubCategory
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		
		public int DisplayOrder { get; set; }
		public int CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		public Category Category { get; set; }
	}
}
