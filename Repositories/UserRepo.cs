using Microsoft.EntityFrameworkCore;
using OAuth.Models;

namespace OAuth.Repositories
{
    public interface IUserRepo
	{
		Task<User> GetUserByUserNameAsync(string username);
	}
	public class UserRepo : IUserRepo
	{
		private readonly OAuthDatabaseContext _context;
		public UserRepo(OAuthDatabaseContext context)
		{
			_context = context;
		}

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            // Retrieve a user from the database based on the provided username
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

    }
}
