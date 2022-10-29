using Data.Entities;

namespace Data
{
	public interface IAppUserRepository
	{
		public Task<AppUserEntity> GetUserByIdAsync(string id);
		public Task<AppUserEntity> GetuserByCredentialsAsync(string username, string password);
	}
}
