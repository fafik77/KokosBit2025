using Microsoft.AspNetCore.Identity;

namespace kokos.Api.Models
{
	public class User : IdentityUser
	{
		public string? Initials { get; set; }
	}
}
