using Microsoft.Extensions.Logging;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureLogging((hostingContext, loggingbuilder) =>
            {
                loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingbuilder.AddConsole();
                loggingbuilder.AddDebug();
            });

            services.AddOcelot()
                .AddCacheManager(settings => settings.WithDictionaryHandle());

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}