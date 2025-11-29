using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kokos.Api.Models
{
	public class UserDBContext : IdentityDbContext<User>
	{
		public UserDBContext(DbContextOptions<UserDBContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>().Property(p => p.Initials).HasMaxLength(3);
			builder.HasDefaultSchema("identity");
		}
	}
}
