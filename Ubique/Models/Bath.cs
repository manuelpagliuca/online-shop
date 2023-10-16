using System.ComponentModel.DataAnnotations;

namespace Ubique.Models
{
	public class Bath
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public int DisplayOrder { get; set; }
	}
}
