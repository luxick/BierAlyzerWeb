using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BierAlyzerWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(48);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                #region Routes

                // AccountController
                routes.MapRoute("Login", "login", new {controller = "Account", action = "Login"});
                routes.MapRoute("Logout", "logout", new {controller = "Account", action = "Logout"});
                routes.MapRoute("SignUp", "signup", new {controller = "Account", action = "SignUp"});

                // HomeController
                routes.MapRoute("Events", "events", new {controller = "Home", action = "Events"});
                routes.MapRoute("Event", "event", new {controller = "Home", action = "Event"});
                routes.MapRoute("UserProfile", "profile", new {controller = "Home", action = "UserProfile"});
                routes.MapRoute("JoinPublicEvent", "joinpublic", new {controller = "Home", action = "JoinPublicEvent"});
                routes.MapRoute("BookDrink", "book", new {controller = "Home", action = "BookDrink"});
                routes.MapRoute("LeaveEvent", "leave", new {controller = "Home", action = "LeaveEvent"});

                // ManagementController
                routes.MapRoute("ManageEvents", "manageevents", new {controller = "Management", action = "Events"});
                routes.MapRoute("ManageEvent", "manageevent", new {controller = "Management", action = "Event"});
                routes.MapRoute("ManageUsers", "manageusers", new {controller = "Management", action = "Users"});
                routes.MapRoute("ManageDrinks", "managedrinks", new {controller = "Management", action = "Drinks"});
                routes.MapRoute("ManageDrink", "managedrink", new {controller = "Management", action = "Drink"});
                routes.MapRoute("SetEventType", "seteventtype",
                    new {controller = "Management", action = "SetEventType"});
                routes.MapRoute("SetEventStatus", "seteventstatus",
                    new {controller = "Management", action = "SetEventStatus"});
                routes.MapRoute("ToggleDrinkVisibility", "drinkvisibility",
                    new {controller = "Management", action = "ToggleDrinkVisibility"});
                routes.MapRoute("ToggleUserEnabled", "UserEnabled",
                    new {controller = "Management", action = "ToggleUserEnabled"});

                // PublicController
                routes.MapRoute("Error", "error", new {controller = "Public", action = "Error"});

                // Default
                routes.MapRoute("default", "{controller}/{action}", new {controller = "Home", action = "Events"});
                routes.MapRoute("fallback", "{*url}", new {controller = "Home", action = "Events"});

                #endregion
            });
        }
    }
}