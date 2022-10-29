using AuthenticationNet6.Models;
using AuthenticationNet6.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNet6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


		public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> AuthenticationAsync(AuthenticateRequest model)
        {
            var response = await _userService.AuthenticateAsync(model);
            return response == null ? BadRequest(new { message = "Username or password is incorrect" }) : Ok(response);
        }
    }
}
