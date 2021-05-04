using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace testKonsolenProjekt
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                //.Enrich.FromLogContext()
                //.WriteTo.Console(new RenderedCompactJsonFormatter())
                //.WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day,
                //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            try
            {
                Log.Information("Starting up");
                await CreateHostBuilder(args).RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            //var service = ActivatorUtilities.CreateInstance<MainStartService>(host.Services);
            //service.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                    .ConfigureServices((context, services) =>
                    {
                        //services.AddTransient<IMainStartService, MainStartService>();
                        services
                            .AddHostedService<ConsoleHostedService>()
                            .AddSingleton<IWeatherService, WeatherService>();

                        services.AddOptions<WeatherSettings>().Bind(context.Configuration.GetSection("Weather"));
                    })
                    .UseSerilog()
                    ;

        public static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                optional: true)
                .AddEnvironmentVariables()
                ;
        }
    }
}
