using AuthenticationNet6.Services;
using Data;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationNet6
{
	public static class ContainerHelper
	{
		public static void RegisterServices(IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddScoped<IAppUserRepository, UserRepository>();
			services.AddScoped<IUnitiyOfWork, UnitOfWork>();
			services.AddScoped<IUserService, UserService>();
			SetupEntityFramework(services, configuration);
		}

		private static void SetupEntityFramework(IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddDbContext<EFAppContext>(options =>
				  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
									  , x => x.MigrationsAssembly("EFDataAccess")
									  ));
		}
	}
}
