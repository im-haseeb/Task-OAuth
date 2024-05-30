using OAuth.Enums;
using OAuth.Models;

namespace OAuth.Seeders
{
    public static class SeedUsers
	{
		public static async Task CreateDefaultUsers(OAuthDatabaseContext _context)
		{
			var users = new List<User>();
			users.Add(new User("admin@game.com", BCrypt.Net.BCrypt.HashPassword("admin"), new List<UserRole> { UserRole.Admin}));
			users.Add(new User("normaluser@game.com", BCrypt.Net.BCrypt.HashPassword("normal"), new List<UserRole> { UserRole.Player }));
			users.Add(new User("vipuser@game.com", BCrypt.Net.BCrypt.HashPassword("vip"), new List<UserRole> { UserRole.Player, UserRole.Vip}));

			CreateUser(users, _context);
		}

		private static async void CreateUser(List<User> users, OAuthDatabaseContext _context)
		{
			try
			{
				if(!_context.Users.Any())
				{
					_context.Users.AddRange(users);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
