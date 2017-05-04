using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure;
using Ubora.Web.Models;
using Ubora.Web.Services;
using Serilog;
using System.IO;

namespace Ubora.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.RollingFile(Path.GetFullPath(Path.Combine("log", "log-{Date}.txt")))
                .CreateLogger();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("ApplicationDbConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddMvc()
                .AddUboraFeatureFolders();

			services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
			    {
			        o.Password.RequireNonAlphanumeric = false;
			    })
                .AddUserManager<ApplicationUserManager>()
                .AddClaimsPrincipalFactory<ApplicationClaimsPrincipalFactory>()
				.AddEntityFrameworkStores<ApplicationDbContext, Guid>()
				.AddDefaultTokenProviders();

            var autofacContainerBuilder = new ContainerBuilder();

            var domainModule = new DomainAutofacModule(connectionString);
            var webModule = new WebAutofacModule();
            autofacContainerBuilder.RegisterModule(domainModule);
            autofacContainerBuilder.RegisterModule(webModule);

            services.AddAutoMapper(cfg => domainModule.AddAutoMapperProfiles(cfg));

            autofacContainerBuilder.Populate(services);
            var container = autofacContainerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (Configuration["AWS_ACCESS_KEY_ID"] != null || Configuration["AWS_SECRET_ACCESS_KEY"] != null)
            {
                loggerFactory.AddAWSProvider(Configuration.GetAWSLoggingConfigSection());
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

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

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
            }
        }
    }
}
