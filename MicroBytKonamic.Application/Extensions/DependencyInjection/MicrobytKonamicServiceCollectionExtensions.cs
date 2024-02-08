using MicroBytKonamic.Application.Dto;
using MicroBytKonamic.Application.Services;
using MicroBytKonamic.Commom.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class MicrobytKonamicServiceCollectionExtensions
{
    public static IServiceCollection AddMicrobytKonamic(this IServiceCollection services)
    {
        services.AddDbContext<MicrobytkonamicContext>();

        // Register & configure automapper
        services.AddAutoMapper(typeof(DtoMappingProfile));

        // Services
        services.AddSingleton<FortuneOfDayContainer>();
        services.AddScoped<IRandomServices, RandomServices>();
        services.AddScoped<ILanguagesServices, LanguagesServices>();
        services.AddScoped<IPostalesServices, PostalesServices>();
        services.AddScoped<IImportFortunesServices, ImportFortunesServices>();
        services.AddScoped<IFortunesServices, FortunesServices>();
        services.AddScoped<IFortuneOfDayServices, FortuneOfDayServices>();
        services.AddHostedService<FortuneOfDayBackgroundService>();

        return services;
    }
}
