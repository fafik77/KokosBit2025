using Microsoft.EntityFrameworkCore;

namespace kokos.Api.Models
{
	public class TodoDBContext : DbContext
	{
		public TodoDBContext(DbContextOptions<TodoDBContext> options)
			: base(options)
		{
		}

		public DbSet<TodoItem> TodoItems { get; set; } = null!;
	}
}
