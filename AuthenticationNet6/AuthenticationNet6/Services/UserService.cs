using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationNet6.Models;
using Data;
using Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationNet6.Services
{
	public class UserService : IUserService
	{
        private readonly AppSettings.Authentication _authenticationSetings;
		private readonly IUnitiyOfWork _unitiyOfWork;
		private readonly IAppUserRepository _appUserRepository;

        public UserService(IOptions<AppSettings.Authentication> authenticationSetings, IUnitiyOfWork unitiyOfWork)
		{
            _authenticationSetings = authenticationSetings.Value;
			_unitiyOfWork = unitiyOfWork;
			_appUserRepository = unitiyOfWork.AppUserRepository;
		}

		public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
		{
			var user = await _appUserRepository.GetuserByCredentialsAsync(model.Username, model.Password);

			// return null if user not found
			if (user == null)
			{
				return null;
			}

			// authentication successful so generate jwt token
			var token = GenerateJwtToken(user);

			return new AuthenticateResponse(user, token);
		}


		public async Task<AppUserEntity> GetByIdAsync(string id)
		{
			return await _appUserRepository.GetUserByIdAsync(id);
		}

		private string GenerateJwtToken(AppUserEntity user)
		{
			// generate token that is valid for 7 days
			var tokenHandler = new JwtSecurityTokenHandler();
			if (_authenticationSetings?.Secret == null)
			{
				throw new Exception($"Expected: {nameof(_authenticationSetings.Secret)}");
			}

			var key = Encoding.ASCII.GetBytes(_authenticationSetings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
