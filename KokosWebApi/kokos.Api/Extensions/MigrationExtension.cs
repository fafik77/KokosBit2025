using kokos.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kokos.Api.Extensions
{
	public static class MigrationExtension
	{
		public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var dbContextEventUser = scope.ServiceProvider.GetRequiredService<UserEventDbContext>();
				//var dbContextTodo = scope.ServiceProvider.GetRequiredService<TodoDBContext>();
				//var dbContextUser = scope.ServiceProvider.GetRequiredService<UserDBContext>();
				// Only apply to real DB, do not apply to inMemory DB
				if (dbContextEventUser.Database.IsRelational())
					dbContextEventUser.Database.Migrate();
				//if (dbContextTodo.Database.IsRelational())
				//	dbContextTodo.Database.Migrate();
				//if (dbContextUser.Database.IsRelational())
				//	dbContextUser.Database.Migrate();
				//app.ApplyMigrations();
				//var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				//if (!await roleMgr.RoleExistsAsync(Roles.Admin))
				//	await roleMgr.CreateAsync(new IdentityRole(Roles.Admin));
				//if (!await roleMgr.RoleExistsAsync(Roles.User))
				//	await roleMgr.CreateAsync(new IdentityRole(Roles.User));
				//if (!await roleMgr.RoleExistsAsync(Roles.SuspendedUser))
				//	await roleMgr.CreateAsync(new IdentityRole(Roles.SuspendedUser));
				//if (!await roleMgr.RoleExistsAsync(Roles.Moderator))
				//	await roleMgr.CreateAsync(new IdentityRole(Roles.Moderator));
			}
		}
	}
}
