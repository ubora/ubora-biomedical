using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Ubora.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.RollingFile(Path.GetFullPath(Path.Combine("log", "log-{Date}.txt")))
                .CreateLogger();

            try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception e)
            {
                try
                {
                    Log.Fatal(e, "Application terminated with an error!");
                }
                catch
                {
                    // ignored
                }
                throw;
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights();

            if (args == null || args.Length > 0)
            {
                hostBuilder.UseUrls(args[1]);
            }

            var host = hostBuilder.Build();

            return host;
        }
    }
}