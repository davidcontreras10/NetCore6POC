using AuthenticationNet6.Models;
using AuthenticationNet6.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationNet6
{
	public class AuthenticationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly AppSettings.Authentication _authenticationSetings;

		public AuthenticationMiddleware(RequestDelegate next, IOptions<AppSettings.Authentication> authenticationSetings)
		{
			_next = next;
			_authenticationSetings = authenticationSetings.Value;
		}

		public async Task Invoke(HttpContext context, IUserService userService)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (token != null)
			{
				await AttachUserToContextAsync(context, userService, token);
			}

			await _next(context);
		}

		private async Task AttachUserToContextAsync(HttpContext context, IUserService userService, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				if (_authenticationSetings?.Secret == null)
				{
					throw new Exception($"Expected: {nameof(_authenticationSetings.Secret)}");
				}

				var key = Encoding.ASCII.GetBytes(_authenticationSetings.Secret);
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

				// attach user to context on successful jwt validation
				context.Items["User"] = await userService.GetByIdAsync(userId);
				var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "Custom");
				context.User = new ClaimsPrincipal(claimsIdentity);
			}
			catch
			{
				// do nothing if jwt validation fails
				// user is not attached to context so request won't have access to secure routes
			}
		}
	}
}
