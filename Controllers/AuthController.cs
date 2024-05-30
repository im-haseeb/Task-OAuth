using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OAuth.Requests;
using OAuth.Models;
using OAuth.Services;

namespace OAuth.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;

		public AuthController(IUserService userService)
		{
			_userService = userService;
		}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Requests.LoginRequest model)
        {
            // Encapsulated business logic in the UserService using Dependency Injection to promote abstraction and separation of concerns
            var response = await _userService.Login(model);
            return StatusCode(response.StatusCode, response);
        }
    }
}
