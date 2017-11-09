using System;
using System.Net.Sockets;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
using Npgsql;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web
{
    public class Startup
    {
        private string ConnectionString => Configuration.GetConnectionString("ApplicationDbConnection");

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

            var npgSqlConnectionString = new NpgsqlConnectionStringBuilder(ConnectionString);

            var isListeningPostgres = WaitForHost(npgSqlConnectionString.Host, npgSqlConnectionString.Port, TimeSpan.FromSeconds(15));
            if (!isListeningPostgres)
            {
                throw new Exception("Database (Postgres) could not be connected to.");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(ConnectionString));

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
                .AddEntityFrameworkStores<ApplicationDbContext>()
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
                services.AddSingleton<TestMentorSeeder>();
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

            var domainModule = new DomainAutofacModule(ConnectionString, storageProvider);
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
            var logger = loggerFactory.CreateLogger<Startup>();

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

            app.UseAuthentication();

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

                var applicationDbContext = serviceProvider.GetService<ApplicationDbContext>();
                applicationDbContext.Database.Migrate();

                var domainMigrator = serviceProvider.GetService<DomainMigrator>();
                domainMigrator.MigrateDomain(ConnectionString);

                var documentStore = serviceProvider.GetService<IDocumentStore>();
                documentStore.Schema.WritePatchByType("Patches");

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

            // Logging this as an error so it reaches all loggers (for tracking application restarts and testing if logging actually works)
            logger.LogError("Application started!");
        }

        //Wait and try to connect a remote TCP host for synchronizing. (Tcp​Client.​Connect method for synchronizing only available in CORE 2.0)
        private bool WaitForHost(string server, int port, TimeSpan timeout)
        {
            using (TcpClient client = new TcpClient())
            {
                var connected = false;
                var timeoutTime = DateTime.Now.AddSeconds(timeout.Seconds);
                while (!connected && DateTime.Now < timeoutTime)
                {
                    try
                    {
                        client.ConnectAsync(server, port).Wait(timeout);
                        connected = true;
                    }
                    catch
                    {
                        connected = false;
                    }
                }
                return connected;
            }
        }
    }
}
