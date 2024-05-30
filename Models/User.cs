using OAuth.Enums;
using System.ComponentModel.DataAnnotations;

namespace OAuth.Models
{
	public class User : BaseModel
	{
		[Required]
        public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public IList<UserRole> Roles { get; set; }

        public User()
        {
            
        }

        public User(string username, string password, IList<UserRole> roles)
        {
            Username = username;
            Password = password;
            Roles = roles;
        }
    }
}
