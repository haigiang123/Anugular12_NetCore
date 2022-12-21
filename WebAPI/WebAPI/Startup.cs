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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sample API",
                    Description = "Sample API for Swagger integration",
                    TermsOfService = new Uri("https://test.com/terms"), // Add url of term of service details
                    Contact = new OpenApiContact
                    {
                        Name = "Test Contact",
                        Url = new Uri("https://test.com/contact") // Add url of contact details
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Test License",
                        Url = new Uri("https://test.com/license") // Add url of license details
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

            });


            #region DI
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
            services.AddTransient<IProductPaternService, ProductPaternService>();
            services.AddTransient<IProductService, ProductService>();
            #endregion
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API");
            });
        }
    }
}
