using kokos.Api.DTO.Types;
using kokos.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace kokos.Api.DTO
{
	public class OpinionsForUserDto
	{
		[Key]
		public long Id { get; set; }

		public string Komentarz { get; set; }

		// 1. Enforce Range [0-10]
		[Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
		public int Rating { get; set; }

		// 2. The Author (Who wrote the review)
		// We don't necessarily need a list of "WrittenReviews" in UserSimple, so we don't use InverseProperty here.
		public UserIdLogin Autor { get; set; }
	}
}
