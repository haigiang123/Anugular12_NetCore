using AdminApp.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebViewModel.SystemService.User;

namespace AdminApp
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
            services.AddHttpClient();

            services.AddControllers()
               .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            services.AddSession(x=> {
                x.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login/Login";
                options.AccessDeniedPath = "/Login/Forbidden/";
            });

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddHttpClient();
            /*IMvcBuilder builder = services.AddRazorPages();
            builder.AddRazorRuntimeCompitation();*/
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "login", pattern: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}
