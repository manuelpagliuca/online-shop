using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ubique.Models
{
	public class SubCategory
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Il campo \"Nome Sotto Categoria\" è obbligatorio.")]
		[MaxLength(30, ErrorMessage = "Il campo \"Nome Sotto Categoria\" deve essere al massimo di 30 caratteri.")]
		[DisplayName("Nome SottoCategoria")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Il campo \"Categoria\" è obbligatorio.")]
		[DisplayName("Categoria")]
		public int CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category? Category { get; set; }
	}
}
