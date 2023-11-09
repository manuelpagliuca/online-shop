using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ubique.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Il campo \"Nome Categoria\" è obbligatorio.")]
		[MaxLength(30, ErrorMessage = "Il campo \"Nome Categoria\" deve essere al massimo di 30 caratteri.")]
		[DisplayName("Nome Categoria")]
		public string Name { get; set; }
	}
}
