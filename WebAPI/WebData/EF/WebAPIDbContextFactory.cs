using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;


namespace WebData.EF
{
    public class WebAPIDbContextFactory : IDesignTimeDbContextFactory<WebAPIDbContext>
    {
        public WebAPIDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuration.GetConnectionString("WebAPIDb");

            var dbContextOptions = new DbContextOptionsBuilder<WebAPIDbContext>();
            dbContextOptions.UseSqlServer(connectionString);

            return new WebAPIDbContext(dbContextOptions.Options);
        }
    }
}
