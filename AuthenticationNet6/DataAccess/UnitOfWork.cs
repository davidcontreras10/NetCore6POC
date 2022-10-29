using Data;

namespace EFDataAccess
{
    public class UnitOfWork : IUnitiyOfWork
    {

        private readonly EFAppContext _appContext;

        public UnitOfWork(IAppUserRepository appUserRepository, EFAppContext appContext)
        {
            AppUserRepository = appUserRepository;
            _appContext = appContext;
        }

        public IAppUserRepository AppUserRepository { get; private set; }

        public async Task CommitChangesAsync()
        {
            await _appContext.SaveChangesAsync();
        }
    }
}
