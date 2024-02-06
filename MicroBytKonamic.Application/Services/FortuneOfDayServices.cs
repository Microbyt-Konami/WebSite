using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class FortuneOfDayServices
{
    private const string _fortuneOfDayCacheKey = "FortuneOfDay";
    private readonly ILanguageServices _languageServices;
    private readonly IMemoryCache _cache;

    public FortuneOfDayServices(ILanguageServices languageServices, IMemoryCache memoryCache)
    {
        _languageServices = languageServices;
        _cache = memoryCache;
    }

    public FortuneOfDayCache? GetFortuneOfDayCache(DateTime day)
    {
        FortuneOfDayCache? fortune = null;

        if (_cache.TryGetValue(_fortuneOfDayCacheKey, out fortune))
        {
            if (fortune!.Day == day)
                return fortune;

            _cache.Remove(_fortuneOfDayCacheKey);
            fortune = null;
        }

        return fortune;
    }

    public void SetFortuneOfDayCache(FortuneOfDayCache fortune) => _cache.Set(_fortuneOfDayCacheKey, fortune);

    public FortuneOfDayCache LoadFortuneOfDayFromBD(DateTime day)
    {
        var fortunes = new FortuneOfDayCache
        {
            Day = day,
            LanguagesFortunes = new()
        };

        foreach(var language in _languageServices.SupportedLanguages)
        {

        }

        return fortunes;
    }
}
