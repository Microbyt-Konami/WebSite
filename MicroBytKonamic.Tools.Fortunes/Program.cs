// https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/

using MicroBytKonamic.Tools.Fortunes;
using MicroBytKonamic.Tools.Fortunes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.File;

HostApplicationBuilder builder = Host.CreateApplicationBuilder
(
    new HostApplicationBuilderSettings
    {
        Args = args,
#if DEBUG
        EnvironmentName = Environments.Development
#else
        EnvironmentName = Environments.Production
#endif
    }
    );

builder.Services.AddLogging
(
    loggingBuilder =>
          loggingBuilder
            //.ClearProviders()
            //.AddDebug()
            .AddSerilog
             (
                new LoggerConfiguration()
                    .WriteTo.File("fortunes/fortunes.log", rollingInterval: RollingInterval.Day)
                    .CreateLogger(),
                dispose: true
             )
);
builder.Services.AddLocalization();
builder.Services.AddSingleton<IResourcesServices, ResourcesServices>();
builder.Services.AddSingleton<JsonFortunesServices>();
builder.Services.AddMicrobytKonamic();
builder.Services.AddScoped<App>();

using IHost host = builder.Build();

var scope = host.Services.CreateScope();

var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
//var hostenvv = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
var log = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//string? x = conf.GetValue<string>("a");

try
{
    await scope.ServiceProvider.GetRequiredService<App>().RunAsync(args);
}
catch (Exception ex)
{
    log.LogError(ex, "App termination");
    //Console.WriteLine(e.Message);
}