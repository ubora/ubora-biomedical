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
using Microsoft.AspNetCore.Mvc;

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

            var isListeningPostgres = WaitForHost(npgSqlConnectionString.Host, npgSqlConnectionString.Port,
                TimeSpan.FromSeconds(15));
            if (!isListeningPostgres)
            {
                throw new Exception("Database (Postgres) could not be connected to.");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(ConnectionString));

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.AddStringTrimmingProvider();
                })
                .AddUboraFeatureFolders(new FeatureFolderOptions {FeatureFolderName = "_Features"})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<Pandoc>(Configuration.GetSection("Pandoc"));

            var useSpecifiedPickupDirectory =
                Convert.ToBoolean(Configuration["SmtpSettings:UseSpecifiedPickupDirectory"]);

            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddUserManager<ApplicationUserManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddClaimsPrincipalFactory<ApplicationClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/access-denied";
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper();
            services.AddUboraPolicyBasedAuthorization();
            services.AddNodeServices(setupAction => setupAction.InvocationTimeoutMilliseconds = 300000);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            if (IsDevelopment)
            {
                services.AddSingleton<TestDataSeeder>();
                services.AddSingleton<TestUserSeeder>();
                services.AddSingleton<TestProjectSeeder>();
                services.AddSingleton<TestMentorSeeder>();
                services.AddMiniProfiler(options =>
                {
                    options.IgnoredPaths.Add("dist");
                    options.IgnoredPaths.Add("images");
                }).AddEntityFramework();
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
                var linkLocalIpAddress = Configuration.GetValue<string>("Storage:LinkLocalIpAddress");
                storageProvider = new CustomDevelopmentAzureStorageProvider(azOptions, azureStorageProvider, linkLocalIpAddress);
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
                //For more details on using MiniProfiler https://miniprofiler.com/dotnet/AspDotNetCore
                app.UseMiniProfiler();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error/");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");

                //routes.MapRoute(
                //    name: "areaRoute",
                //    template: "{area:exists}/{controller}/{action}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                var applicationDbContext = serviceProvider.GetService<ApplicationDbContext>();
                applicationDbContext.Database.Migrate();

                var documentStore = serviceProvider.GetService<IDocumentStore>();
                var domainMigrator = serviceProvider.GetService<DomainMigrator>();

                domainMigrator.MigrateDomain(ConnectionString);
                var patchFilename = DateTime.Now.ToString("dd-MM-yyyy") + "-marten-automatic-patch.sql";
                documentStore.Schema.WritePatch(patchFilename);

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
                var timeoutTime = DateTime.Now.AddSeconds(timeout.Seconds);
                while (DateTime.Now < timeoutTime)
                {
                    try
                    {
                        return client.ConnectAsync(server, port).Wait(timeout);
                    }
                    catch
                    {
                        if (DateTime.Now > timeoutTime)
                            throw;
                    }
                }
                return false;
            }
        }
    }
}