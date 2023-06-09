using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

namespace OcelotApiGw
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureLogging((hostingContext, loggingbuilder) =>
            {
                loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingbuilder.AddConsole();
                loggingbuilder.AddDebug();
            });

            builder.WebHost.UseStartup<Program>();

            builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
            });

            builder.Services.AddOcelot();

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            await app.UseOcelot();

            app.Run();
        }
    }
}