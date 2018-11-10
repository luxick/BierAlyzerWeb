using System;
using System.Linq;
using BierAlyzerWeb.Helper;
using Contracts.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BierAlyzerWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = ContextHelper.OpenContext())
            {
                #region EnsureAdmin

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

                #endregion
            }

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseStartup<Startup>()
                .Build();
        }
    }
}