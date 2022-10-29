using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess
{
	public class EFAppContext : DbContext
	{
		public EFAppContext() { }
		public EFAppContext(DbContextOptions<EFAppContext> options) : base(options) { }

		public DbSet<AppUserEntity> AppUsers { get; set; }
	}
}
