using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BierAlyzerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var host = CreateWebHostBuilder(args).Build())
            {
                var config = host.Services.GetService<IConfiguration>();

                try
                {
                    host.Run();
                }
                finally
                {
                    // TODO: Perform log
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
