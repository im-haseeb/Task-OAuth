using Microsoft.IdentityModel.Tokens;
using OAuth.Enums;
using OAuth.Models;
using OAuth.Repositories;
using OAuth.Requests;
using OAuth.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OAuth.Services
{
    public interface IUserService
	{
		Task<CustomResponse> Login(Requests.LoginRequest model);
	}
	public class UserService : IUserService
	{
		private readonly IUserRepo _userRepo;
		public UserService(IUserRepo userRepo)
		{
			_userRepo = userRepo;
		}
        public async Task<CustomResponse> Login(LoginRequest model)
        {
            try
            {
                // Retrieve user from the repository using the provided username
                var user = await _userRepo.GetUserByUserNameAsync(model.Username);

                // If user is not found, return a custom response with a 404 status code
                if (user is null)
                {
                    return new CustomResponse(404, "User not found");
                }

                // Verify the provided password against the hashed password stored in the user object
                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    // If password verification fails, return a custom response with a 409 status code
                    return new CustomResponse(409, "Username or password is invalid");
                }

                // Determine the scopes (permissions) associated with the user's roles
                var scopes = GetScopesByRoles(user.Roles.ToList());

                // Generate a JWT token containing user information and scopes
                var token = GenerateJwtToken(user, scopes);

                // Construct a custom response with a 200 status code, indicating successful login
                // Include user data, token, and any additional information as part of the response
                return new CustomResponse(200, "Logged in successfully.", new
                {
                    userData = GetUserInfo(user, scopes),
                    token = token,
                });
            }
            catch (Exception ex)
            {
                // If an unexpected exception occurs, return a custom response with a 500 status code
                return new CustomResponse(500, ex.InnerException?.Message ?? ex.Message);
            }
        }



        private string GenerateJwtToken(User user, List<string> scopes)
        {
            // Define security key and signing credentials for JWT token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hie6f7QzIGpP1BrJzzqKLhSn2E3Qc2Q2Gd4yXM0w1So2zUu2fWYbSfzTZFnJ5iw6iJzO8SCSEvDDcP+oh/YgVQ=="));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Initialize claims list with user's name
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username)
    };

            // Add user's roles to claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            // Add distinct scopes to claims
            foreach (var scope in scopes.Distinct())
            {
                claims.Add(new Claim("scope", scope));
            }

            // Generate JWT token with specified issuer, audience, expiration time, claims, and signing credentials
            var token = new JwtSecurityToken(
                issuer: "by_OAuth",
                audience: "Oauth_",
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                claims: claims,
                signingCredentials: credentials
            );

            // Write the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private UserViewModel GetUserInfo(User user, List<string> scopes)
        {
            var userInfo = new UserViewModel();
            userInfo.Id = user.Id;
            userInfo.Username = user.Username;
            userInfo.Roles = user.Roles.Select(r => r.ToString()).ToList();
            userInfo.Scopes = scopes.Distinct().ToList();
            return userInfo;
        }

        private List<string> GetScopesByRoles(List<UserRole> roles)
		{
			var scopes =  new List<string>();

			if(roles.Contains(UserRole.Player))
			{
				var playerScopes = new List<string> { "b_game" };
				scopes.AddRange(playerScopes);
			}
            if (roles.Contains(UserRole.Admin))
            {
				var adminScopes = new List<string> { "b_game", "global", "admin_routes" };
                scopes.AddRange(adminScopes);
            }
            if (roles.Contains(UserRole.Vip))
            {
                var vipPlayerScopes = new List<string> { "b_game", "vip_chararacter_personalize" };
                scopes.AddRange(vipPlayerScopes);
            }
			return scopes;
		}

	}
}
