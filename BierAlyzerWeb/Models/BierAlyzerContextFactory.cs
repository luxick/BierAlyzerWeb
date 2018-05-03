using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BierAlyzerWeb.Models
{
    public class BierAlyzerContextFactory : IDesignTimeDbContextFactory<BierAlyzerContext>
    {
        #region CreateDbContext

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates database context. </summary>
        /// <remarks>   Andre Beging, 29.04.2018. </remarks>
        /// <param name="args"> The arguments. </param>
        /// <returns>   The new database context. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public BierAlyzerContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BierAlyzerContext>();

            var connectionString = configuration.GetConnectionString("Database");

            builder.UseSqlite(connectionString);

            return new BierAlyzerContext(builder.Options);
        }

        #endregion
    }
}