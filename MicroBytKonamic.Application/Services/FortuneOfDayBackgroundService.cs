using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class FortuneOfDayBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IServiceScope _serviceScope;
    private readonly IServiceProvider _serviceProvider;
    private readonly IFortuneOfDayServices _fortuneOfDayServices;

    public FortuneOfDayBackgroundService(IServiceScopeFactory serviceScopeFactory/*, IServiceScope serviceScope*/)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _serviceScope = _serviceScopeFactory.CreateScope();
        //_serviceScope = serviceScope;
        _serviceProvider = _serviceScope.ServiceProvider;
        _fortuneOfDayServices = _serviceProvider.GetRequiredService<IFortuneOfDayServices>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _fortuneOfDayServices.LoadFortuneOfDayIntoContainerAsync(DateTime.Now, stoppingToken);
    }

    public override void Dispose()
    {
        base.Dispose();
        _serviceScope.Dispose();
    }
}
