using AuthenticationNet6.Models;
using Data.Entities;

namespace AuthenticationNet6.Services
{
	public interface IUserService
	{
		Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
		Task<AppUserEntity> GetByIdAsync(string id);
	}
}
