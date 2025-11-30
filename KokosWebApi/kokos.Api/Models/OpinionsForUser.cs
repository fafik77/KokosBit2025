using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kokos.Api.Models
{
	public class OpinionsForUser
	{
		[Key]
		public long Id { get; set; }

		public string Komentarz { get; set; }

		// 1. Enforce Range [1-10]
		[Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
		public int Rating { get; set; }

		// 2. The Author (Who wrote the review)
		// We don't necessarily need a list of "WrittenReviews" in UserSimple, so we don't use InverseProperty here.
		public UserSimple Autor { get; set; }

		// 3. The Subject (Who is being reviewed)
		// This connects to the list 'OpinionsForUser' in the UserSimple class
		public int UserId { get; set; } // Foreign Key

		[ForeignKey("UserId")]
		[InverseProperty("OpinionsForUser")] // Points to the list in UserSimple
		public UserSimple RatedUser { get; set; }
	}
}
