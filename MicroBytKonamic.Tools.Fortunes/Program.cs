// https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/

using MicroBytKonamic.Tools.Fortunes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

builder.Services.AddLocalization();
builder.Services.AddSingleton<IResourcesServices, ResourcesServices>();
builder.Services.AddMicrobytKonamic();
builder.Services.AddScoped<App>();

using IHost host = builder.Build();

var scope = host.Services.CreateScope();

var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
var hostenvv = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

string? x = conf.GetValue<string>("a");

try
{
    await scope.ServiceProvider.GetRequiredService<App>().RunAsync(args);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}