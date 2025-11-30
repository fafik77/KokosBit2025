using kokos.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kokos.Api.DTO
{
	public class UserSimpleDto
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(32, MinimumLength = 4, ErrorMessage = "Login must be between 4 and 32 characters.")]
		public string Login { get; set; }
		public string? Preferencje { get; set; }

		public int TrwajaceWydarzenia { get; set; }

		public float UserRating { get; set; }
	}
}
