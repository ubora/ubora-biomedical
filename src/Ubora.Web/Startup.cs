using System;
using System.IO;
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
using TwentyTwenty.Storage.Local;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
        }

        public IConfigurationRoot Configuration { get; }

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

            services.AddSingleton<ApplicationDataSeeder>();
            services.AddSingleton<AdminSeeder>();
            services.Configure<AdminSeeder.Options>(Configuration.GetSection("InitialAdminOptions"));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            var autofacContainerBuilder = new ContainerBuilder();

            IStorageProvider storageProvider;
            var isLocalStorage = Configuration.GetValue<bool?>("Storage:IsLocal") ?? false;
            if (isLocalStorage)
            {
                var basePath = Path.GetFullPath("wwwroot/images/storages");
                storageProvider = new FixedLocalStorageProvider(basePath, new LocalStorageProvider(basePath));
            }
            else
            {
                var options = new AzureProviderOptions
                {
                    ConnectionString = Configuration.GetConnectionString("AzureBlobConnectionString")
                };
                storageProvider = new AzureStorageProvider(options);
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
            }

            var logger = loggerFactory.CreateLogger<Startup>();
            // Logging this as an error so it reaches all loggers (for tracking application restarts and testing if logging actually works)
            logger.LogError("Application started!");
        }
    }
}
