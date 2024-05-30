using Microsoft.AspNetCore.Identity;
using OAuth.Enums;
using OAuth.Models;

namespace OAuth.Responses
{
    public class UserViewModel : BaseModel
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Scopes { get; set; }
    }
}
