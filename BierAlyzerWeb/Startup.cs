using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Css;
using NUglify.JavaScript;

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
            #region Bundle Configurations

            var javascriptBundleSettings = new CodeSettings
            {
                PreserveImportantComments = false,
                MinifyCode = false,
                IgnoreAllErrors = true
            };

            var cssSettings = new CssSettings
            {
                MinifyExpressions = true,
                CommentMode = CssComment.None,
            };

            #endregion

            services.AddMvc();
            services.AddWebOptimizer(pipeline =>
            {
                #region Bundles

                // Global CSS
                pipeline.AddCssBundle(
                    "/css/bieralyzer.min.css",
                    cssSettings,
                    "/css/global/bootstrap-4.1.0.min.css",
                    "/css/global/bootstrap-material-design.min.css",
                    "/css/global/fontawesome-5.0.11.all.css",
                    "/css/global/materialdesign-custom.css",
                    "/css/global/site.css");

                // Typeahead CSS
                pipeline.AddCssBundle(
                    "/css/typeahead.min.css",
                    cssSettings,
                    "/css/typeahead.css");

                // Global JS
                pipeline.AddJavaScriptBundle(
                    "/js/bieralyzer.min.js",
                    javascriptBundleSettings,
                    "/js/global/jquery-3.3.1.slim.min.js",
                    "/js/global/popper-1.14.3.min.js",
                    "/js/global/bootstrap-4.1.0.min.js",
                    "/js/global/bootstrap-material-design.min.js",
                    "/js/global/site.js");

                // Typeahead JS
                pipeline.AddJavaScriptBundle(
                    "/js/typeahead.min.js",
                    javascriptBundleSettings,
                    "/js/typeahead.js",
                    "/js/typeahead-matcher.js");

                #endregion
            });
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
            #region Exception Configuration

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            #endregion

            app.UseStaticFiles();
            app.UseSession();

            app.UseWebOptimizer();

            app.UseMvc(routes =>
            {
                #region Routes

                // AccountController
                routes.MapRoute("Login", "login", new { controller = "Account", action = "Login" });
                routes.MapRoute("Logout", "logout", new { controller = "Account", action = "Logout" });
                routes.MapRoute("SignUp", "signup", new { controller = "Account", action = "SignUp" });

                // HomeController
                routes.MapRoute("Events", "events", new { controller = "Home", action = "Events" });
                routes.MapRoute("Event", "event", new { controller = "Home", action = "Event" });
                routes.MapRoute("UserProfile", "profile", new { controller = "Home", action = "UserProfile" });
                routes.MapRoute("JoinPublicEvent", "joinpublic", new { controller = "Home", action = "JoinPublicEvent" });
                routes.MapRoute("BookDrink", "book", new { controller = "Home", action = "BookDrink" });
                routes.MapRoute("LeaveEvent", "leave", new { controller = "Home", action = "LeaveEvent" });

                // ManagementController
                routes.MapRoute("ManageEvents", "manageevents", new { controller = "Management", action = "Events" });
                routes.MapRoute("ManageEvent", "manageevent", new { controller = "Management", action = "Event" });
                routes.MapRoute("ManageUsers", "manageusers", new { controller = "Management", action = "Users" });
                routes.MapRoute("ManageDrinks", "managedrinks", new { controller = "Management", action = "Drinks" });
                routes.MapRoute("ManageDrink", "managedrink", new { controller = "Management", action = "Drink" });
                routes.MapRoute("SetEventType", "seteventtype",
                    new { controller = "Management", action = "SetEventType" });
                routes.MapRoute("SetEventStatus", "seteventstatus",
                    new { controller = "Management", action = "SetEventStatus" });
                routes.MapRoute("ToggleDrinkVisibility", "drinkvisibility",
                    new { controller = "Management", action = "ToggleDrinkVisibility" });
                routes.MapRoute("ToggleUserEnabled", "UserEnabled",
                    new { controller = "Management", action = "ToggleUserEnabled" });

                // PublicController
                routes.MapRoute("Error", "error", new { controller = "Public", action = "Error" });
                routes.MapRoute("Impressum", "impressum", new { controller = "Public", action = "Impressum" });
                routes.MapRoute("Privacy", "privacy", new { controller = "Public", action = "Privacy" });

                // Default
                routes.MapRoute("default", "{controller}/{action}", new { controller = "Home", action = "Events" });
                routes.MapRoute("fallback", "{*url}", new { controller = "Home", action = "Events" });

                #endregion
            });
        }
    }
}