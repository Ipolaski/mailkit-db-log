using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TestApp.Infrastructure.Entities;

using File = TestApp.Infrastructure.Entities.File;

namespace TestApp.Infrastructure
{
    public class AppContext : DbContext
    {
        public DbSet<File> Files => Set<File>();

        public DbSet<Table2> Table2s => Set<Table2>();
        public bool UseSqlServer = false;
        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                       .SetBasePath(Directory.GetCurrentDirectory())
                                                       .AddJsonFile("appsettings.json", true, true);

            IConfiguration _config = configurationBuilder.Build();
            var posgreConnection = _config.GetConnectionString("PostgreSql");
            var msConnection = _config.GetConnectionString("SqlServer");


            UseSqlServer = string.IsNullOrEmpty(posgreConnection);
            if ( UseSqlServer )
                optionsBuilder.UseSqlServer(msConnection);
            else
                try
                {
                    optionsBuilder.UseNpgsql(posgreConnection);
                }
                catch
                {
                    optionsBuilder.UseSqlServer(msConnection);
                }
        }
    }
}