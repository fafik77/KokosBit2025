using System.ComponentModel.DataAnnotations;

namespace kokos.Api.Models.Types
{
	public class EventCreate
	{
		[Required(ErrorMessage = "Nazwa is required")]
		[StringLength(128, MinimumLength = 4, ErrorMessage = "Nazwa must be between 4 and 128 characters.")]
		public string Nazwa { get; set; }
		public string? Opis { get; set; }
		public int OrganizatorId { get; set; }
		public string Typ { get; set; }
		//osobno
		public DateOnly Data { get; set; }
		public TimeOnly Godzina { get; set; }
		//lokcaja
		public double Wysokosc { get; set; }
		public double Szerokosc { get; set; }
	}
}
