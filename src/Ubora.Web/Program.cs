using System;
using System.IO;
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
                var hostBuilder = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseIISIntegration()
                    .UseApplicationInsights();

                if (args == null || args.Length > 0)
                {
                    hostBuilder.UseUrls(args[0]);
                }

                 var host = hostBuilder.Build();

                 host.Run();
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
    }
}
