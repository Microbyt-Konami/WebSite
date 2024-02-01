using MicroBytKonamic.Application.Dto;
using MicroBytKonamic.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class MicrobytKonamicServiceCollectionExtensions
{
    public static void AddMicrobytKonamic(this IServiceCollection services)
    {
        services.AddDbContext<MicrobytkonamicContext>();

        // Register & configure automapper
        services.AddAutoMapper(typeof(DtoMappingProfile));

        // Services
        services.AddScoped<IPostalesServices, PostalesServices>();
        services.AddScoped<IFortunesServices, FortunesServices>();
    }
}
