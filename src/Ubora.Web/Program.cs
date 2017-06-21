using System;
using System.IO;
using System.Net.Sockets;
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

            var isListeningPostgres = WaitForHost("postgres", 5432, TimeSpan.FromSeconds(15));

            if (!isListeningPostgres)
            {
                Log.Fatal("Postgres not found!");
            }
              
            try
            {     
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseIISIntegration()
                    .UseApplicationInsights()
                    .Build();

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

            //Wait and try to connect a remote TCP host for synchronizing. (Tcp​Client.​Connect method for synchronizing only available in CORE 2.0)
            bool WaitForHost(string server, int port, TimeSpan timeout)
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
}
