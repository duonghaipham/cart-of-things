using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin
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
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                #region SignIn
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=SignIn}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                #endregion

                #region User
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "{controller=User}/{action=Retrieve}");
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "{controller=User}/{action=Create}");
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "{controller=User}/{action=Update}/{Id?}");
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "{controller=User}/{action=Lock}/{Id?}");
                #endregion

                #region Place
                endpoints.MapControllerRoute(
                    name: "place",
                    pattern: "{controller=Place}/{action=Retrieve}");
                endpoints.MapControllerRoute(
                    name: "place",
                    pattern: "{controller=Place}/{action=Update}/{Id?}");
                #endregion
            });
        }
    }
}
