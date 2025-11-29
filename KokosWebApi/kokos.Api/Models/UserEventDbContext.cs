using Microsoft.EntityFrameworkCore;

namespace kokos.Api.Models
{
	public class UserEventDbContext : DbContext
	{
		public UserEventDbContext(DbContextOptions<UserEventDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// 1. Configure the "Organizer" relationship
			// We explicitly say: "Organizator" connects to "Wydarzenia"
			modelBuilder.Entity<EventInfo>()
				.HasOne(e => e.Organizator)
				.WithMany(u => u.Wydarzenia)
				.IsRequired(); // Since Organization is mandatory

			// 2. Configure "Confirmed Participants" (Many-to-Many)
			// Since UserSimple doesn't have a specific list for this, we use .WithMany() empty
			modelBuilder.Entity<EventInfo>()
				.HasMany(e => e.UczestnicyPotwierdzeni)
				.WithMany()
				.UsingEntity(j => j.ToTable("EventParticipantsConfirmed"));

			// 3. Configure "Willing Participants" (Many-to-Many)
			modelBuilder.Entity<EventInfo>()
				.HasMany(e => e.UczestnicyChetni)
				.WithMany()
				.UsingEntity(j => j.ToTable("EventParticipantsWilling"));

			// Make Login Unique
			modelBuilder.Entity<UserSimple>()
				.HasIndex(u => u.Login)
				.IsUnique();
		}

		public DbSet<EventInfo> Wydarzenia { get; set; } = null!;
		public DbSet<UserSimple> Uzytkownicy { get; set; } = null!;
	}
}
