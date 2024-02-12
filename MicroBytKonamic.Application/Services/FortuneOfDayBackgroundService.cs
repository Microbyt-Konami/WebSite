using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public delegate void ChangeTimerHandler(DateTime day);
public delegate Task FortuneOfDayIntoWorkHandler(DateTime day);

public readonly struct FortuneOfDayIntoWorkTimerState(ChangeTimerHandler changeTimer, FortuneOfDayIntoWorkHandler fortuneOfDayIntoWorker)
{
    private readonly ChangeTimerHandler changeTimer = changeTimer;
    private readonly FortuneOfDayIntoWorkHandler fortuneOfDayIntoWorker = fortuneOfDayIntoWorker;

    public Task FortuneOfDayIntoWorker(DateTime day) => fortuneOfDayIntoWorker(day);
    public void ChangeTimer(DateTime day) => changeTimer(day);
}

public class FortuneOfDayBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IServiceScope _serviceScope;
    private readonly IServiceProvider _serviceProvider;
    private readonly IFortuneOfDayServices _fortuneOfDayServices;
    private readonly ILogger<FortuneOfDayBackgroundService> _logger;
    private SemaphoreSlim loopLock = new SemaphoreSlim(1, 1);
    private CancellationTokenSource? cancellationTokenSource;
    private Timer? timer;
    private int? _maxCharSize;

    public FortuneOfDayBackgroundService(IServiceScopeFactory serviceScopeFactory/*, IServiceScope serviceScope*/)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _serviceScope = _serviceScopeFactory.CreateScope();
        //_serviceScope = serviceScope;
        _serviceProvider = _serviceScope.ServiceProvider;
        _fortuneOfDayServices = _serviceProvider.GetRequiredService<IFortuneOfDayServices>();
        _logger = _serviceProvider.GetRequiredService<ILogger<FortuneOfDayBackgroundService>>();
    }

    public static FortuneOfDayBackgroundService Create(IServiceProvider serviceProvider, int maxCharSize)
        => new FortuneOfDayBackgroundService(serviceProvider.GetRequiredService<IServiceScopeFactory>()) { _maxCharSize = maxCharSize };

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (cancellationTokenSource != null)
            await cancellationTokenSource.CancelAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var day = DateTime.Now.Date;

        await FortuneOfDayIntoWorker(day, stoppingToken);
        //await Task.Factory.StartNew(async () => await Loop(), cancellationTokenSource!.Token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        timer = new Timer(FortuneOfDayIntoWorkTimerCallback, new FortuneOfDayIntoWorkTimerState(ChangeTimer, FortuneOfDayIntoWorker), CalcDueTime(day), Timeout.InfiniteTimeSpan);
    }

    public override void Dispose()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
        timer?.Dispose();
        _serviceScope.Dispose();
        loopLock.Dispose();
    }

    private TimeSpan CalcDueTime(DateTime day)
    {
        var nextDay = day.Date.AddDays(1);
        var now = DateTime.Now;
        var diff = nextDay - now;

        return diff < TimeSpan.Zero ? TimeSpan.Zero : diff;
    }

    private void ChangeTimer(DateTime day)
    {
        var diff = CalcDueTime(day);

        if (timer != null)
            timer.Change(diff, Timeout.InfiniteTimeSpan);
    }

    private async Task FortuneOfDayIntoWorker(DateTime day, CancellationToken cancellationToken)
    {
        await loopLock.WaitAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await _fortuneOfDayServices.LoadFortuneOfDayIntoContainerAsync(day, _maxCharSize, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FortuneOfDay don't done");
        }
        finally
        {
            loopLock.Release();
        }
    }

    private async Task FortuneOfDayIntoWorker(DateTime day) => await FortuneOfDayIntoWorker(day, cancellationTokenSource?.Token ?? CancellationToken.None);

    private static async void FortuneOfDayIntoWorkTimerCallback(object? state)
    {
        if (state == null || !(state is FortuneOfDayIntoWorkTimerState _state))
            return;

        var day = DateTime.Now.Date;

        await _state.FortuneOfDayIntoWorker(day);

        _state.ChangeTimer(day);
    }
}
