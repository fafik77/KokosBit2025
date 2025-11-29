using System.ComponentModel.DataAnnotations;

namespace kokos.Api.Models
{
	public class UserSimple
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(32, MinimumLength = 4, ErrorMessage = "Login must be between 4 and 32 characters.")]
		public string Login { get; set; }


		public List<EventInfo>? Wydarzenia { get; set; }
	}
}
