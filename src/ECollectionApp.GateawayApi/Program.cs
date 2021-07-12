using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ECollectionApp.GatewayApi
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, builder) => {
                    builder.AddJsonFile("appsettings.json", false, true);
                    builder.AddJsonFile("appsettings.auth.json", false, true);
                    builder.AddJsonFile("ocelot.json", false, true);
                });
    }
}
