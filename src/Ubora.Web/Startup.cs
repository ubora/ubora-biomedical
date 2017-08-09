using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OdeToCode.AddFeatureFolders;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Authorization;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure;
using Ubora.Web.Services;
using Ubora.Web.Infrastructure.DataSeeding;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Azure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            IsDevelopment = env.IsDevelopment();
        }

        public IConfigurationRoot Configuration { get; }
        private bool IsDevelopment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            var connectionString = Configuration["ConnectionStrings:ApplicationDbConnection"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services
                .AddMvc()
                .AddUboraFeatureFolders(new FeatureFolderOptions { FeatureFolderName = "_Features" });
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            var useSpecifiedPickupDirectory = Convert.ToBoolean(Configuration["SmtpSettings:UseSpecifiedPickupDirectory"]);

            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
                {
                    o.Password.RequireNonAlphanumeric = false;
                })
                .AddUserManager<ApplicationUserManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddClaimsPrincipalFactory<ApplicationClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<ApplicationDbContext, Guid>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper();
            services.AddUboraAuthorization();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            if (IsDevelopment)
            {
                services.AddSingleton<TestDataSeeder>();
                services.AddSingleton<TestUserSeeder>();
                services.AddSingleton<TestProjectSeeder>();
            }

            services.AddSingleton<ApplicationDataSeeder>();
            services.AddSingleton<AdminSeeder>();
            services.Configure<AdminSeeder.Options>(Configuration.GetSection("InitialAdminOptions"));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            var azureBlobConnectionString = Configuration.GetConnectionString("AzureBlobConnectionString");
            var autofacContainerBuilder = new ContainerBuilder();
            var azOptions = new AzureProviderOptions
            {
                ConnectionString = azureBlobConnectionString
            };
            var azureStorageProvider = new AzureStorageProvider(azOptions);
            IStorageProvider storageProvider = null;
            var isLocalStorage = Configuration.GetValue<bool?>("Storage:IsLocal") ?? false;

            if (isLocalStorage)
            {
                storageProvider = new CustomDevelopmentAzureStorageProvider(azOptions, azureStorageProvider);
            }
            else
            {
                storageProvider = new CustomAzureStorageProvider(azOptions, azureStorageProvider);
            }

            var domainModule = new DomainAutofacModule(connectionString, storageProvider);
            var webModule = new WebAutofacModule(useSpecifiedPickupDirectory);
            autofacContainerBuilder.RegisterModule(domainModule);
            autofacContainerBuilder.RegisterModule(webModule);

            autofacContainerBuilder.Populate(services);
            var container = autofacContainerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error/");

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                serviceProvider.GetService<ApplicationDbContext>().Database.Migrate();

                var seeder = serviceProvider.GetService<ApplicationDataSeeder>();
                seeder.SeedIfNecessary()
                    .GetAwaiter().GetResult();

                if (env.IsDevelopment())
                {
                    var testDataSeeder = serviceProvider.GetService<TestDataSeeder>();
                    testDataSeeder.SeedIfNecessary()
                        .GetAwaiter().GetResult();
                }
            }

            var logger = loggerFactory.CreateLogger<Startup>();
            // Logging this as an error so it reaches all loggers (for tracking application restarts and testing if logging actually works)
            logger.LogError("Application started!");
        }
    }
}
