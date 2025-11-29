using kokos.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace kokos.Api.DTO
{
	public class EventInfoWithNamesOnlyDTO
	{
		[Key]
		public long Id { get; set; }

		[Required(ErrorMessage = "Nazwa is required")]
		[StringLength(128, MinimumLength = 4, ErrorMessage = "Nazwa must be between 4 and 128 characters.")]
		public string Nazwa { get; set; }
		public string? Opis { get; set; }
		public string? Organizator { get; set; }
		public string Typ { get; set; }
		//osobno
		public DateOnly Data { get; set; }
		public TimeOnly Godzina { get; set; }
		//lokcaja
		public double Wysokosc { get; set; }
		public double Szerokosc { get; set; }
		public bool Zakonczone { get; set; } = false;

		public List<string>? UczestnicyPotwierdzeni { get; set; }
		public List<string>? UczestnicyChetni { get; set; }

	}


}
