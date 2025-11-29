using System.ComponentModel.DataAnnotations;

namespace kokos.Domain.Entities
{
	public abstract class Entity
	{
		[Key]
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

	}
}
