using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFDataAccess
{
	public class UserRepository : IAppUserRepository
	{
		private readonly EFAppContext _appContext;

		public UserRepository(EFAppContext appContext)
		{
			_appContext = appContext;
		}

		public async Task<AppUserEntity> GetuserByCredentialsAsync(string username, string password)
		{
			return await _appContext.AppUsers.FirstOrDefaultAsync(x => x.Username == username && x.Password == x.Password);
		}

		public async Task<AppUserEntity> GetUserByIdAsync(string id)
		{
			return await _appContext.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
