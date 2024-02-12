// https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/

using MicroBytKonamic.Application.Services;
using MicroBytKonamic.Data.DataContext;
using MicroBytKonamic.Tools.Fortunes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

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
    builder =>
    {
        builder.AddColorMessageConsole(options => options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled);
        //builder.AddSimpleConsole(options =>
        //{
        //    options.SingleLine = true;
        //    options.UseUtcTimestamp = false;
        //    options.IncludeScopes = false;
        //    options.TimestampFormat = "HH:mm:ss ";
        //});
        builder.AddFile("fortunes/fortunes.log", append: false);

    }
);
builder.Services
    .AddLocalization()
    .AddSingleton<IResourcesServices, ResourcesServices>()
    .AddSingleton<JsonFortunesServices>()
    .AddDbContext<MicrobytkonamicContext>()
    .AddScoped<IImportFortunesServices, ImportFortunesServices>()
    .AddScoped<App>();

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