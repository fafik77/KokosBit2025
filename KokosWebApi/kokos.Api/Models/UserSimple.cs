using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kokos.Api.Models
{
	public class UserSimple
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(32, MinimumLength = 4, ErrorMessage = "Login must be between 4 and 32 characters.")]
		public string Login { get; set; }
		public string? Preferencje { get; set; }


		public List<EventInfo>? Wydarzenia { get; set; }

		// This list represents opinions RECEIVED by this user
		[InverseProperty("RatedUser")]
		public List<OpinionsForUser>? OpinionsForUser { get; set; }
	}
}
