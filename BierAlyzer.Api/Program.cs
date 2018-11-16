using System;
using System.Linq;
using BierAlyzer.Api.Models;
using BierAlyzer.Contracts.Model;
using BierAlyzer.EntityModel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BierAlyzer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region EnsureAdmin

            var factory = new BierAlyzerContextFactory();
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var context = factory.CreateDbContext(null))
            {
                

                var defaultAdminMail = "bier@troogs.de";

                var adminUser = context.User.FirstOrDefault(u => u.Mail.ToLower() == defaultAdminMail);
                if (adminUser == null)
                {
                    var user = new User
                    {
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Enabled = true,
                        Hash = "B1B57C0699ED6120AA594127C84DB895",
                        Salt = "71BFDCDED04E94A8939E20A0DB8B174D",
                        Mail = defaultAdminMail,
                        Type = UserType.Admin,
                        Username = "Admin",
                        Origin = "Uni Siegen"
                    };

                    context.User.Add(user);
                }
                else
                {
                    adminUser.Type = UserType.Admin;
                }

                context.SaveChanges();

                
            }

            #endregion

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
                .UseUrls("http://*:5001")
                .UseStartup<Startup>();
    }
}
