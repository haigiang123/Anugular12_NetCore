using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services;
using WebApplication.Common;
using WebApplication.OtherService;
using WebData.EF;
using WebData.Entities;
using WebRepository.Infrastructure;
using WebRepository.Repository;
using WebUltilities;

namespace WebAPI
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
            services.AddControllers();
            services.AddMvc();

            services.AddTransient<ITransient, SomeDI>();
            services.AddScoped<IScope, SomeDI>();
            services.AddSingleton<ISingleton, SomeDI>();

            services.AddDbContext<WebAPIDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString))); ;

            services.AddTransient<IFactory, Factory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddIdentity<AppUser, AppRole>()
               .AddEntityFrameworkStores<WebAPIDbContext>()
               .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductInCategoryRepository, ProductInCategoryRepository>();
            services.AddTransient<IProductTranslationRepository, ProductTranslationRepository>();
            services.AddTransient<ILanguageRepository, LanguageRepository>();
            services.AddTransient<IProductImageRepository, ProductImageRepository>();
            services.AddTransient<ICategoryTranslationRepository, CategoryTranslationRepository>();
            

            services.AddTransient<IStorageService, StorageService>();
            services.AddTransient<IProductService, ProductService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=TestMVC}/{action=Index}");
            });



        }
    }
}
