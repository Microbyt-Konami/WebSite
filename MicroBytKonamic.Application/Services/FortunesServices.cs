using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class FortunesServices : IFortunesServices
{
    private readonly MicrobytkonamicContext _dbContext;
    private readonly IMemoryCache _cache;
    private readonly IRandomServices _random;
    private readonly IMapper _mapper;

    public FortunesServices(MicrobytkonamicContext dbContext, IMemoryCache cache, IRandomServices random, IMapper mapper)
    {
        _dbContext = dbContext;
        _cache = cache;
        _random = random;
        _mapper = mapper;
    }

    public async Task<FortuneOfDayDto> GetFortuneOfDayAsync(string language, CancellationToken cancellationToken)
    {
        var fortuneDto = GetFortuneOfDayCocke(language);

        if (fortuneDto != null)
            return fortuneDto;

        var fortunes0 =
        (
            from f in _dbContext.Fortunes
            join ff in _dbContext.Filesfortunes on f.IdFilesFortunes equals ff.IdFilesFortunes
            join l in _dbContext.Languages on ff.IdLanguages equals l.IdLanguages
            select new { f, l.Culture }
        );
        var fortunes = fortunes0.Where(f => f.Culture == language);
        var count = fortunes.Count();

        if (count == 0)
        {
            fortunes = fortunes0;
            count = fortunes.Count();
        }

        var idx = _random.NetInt(count);
        var fortune = await fortunes.Skip(idx).FirstAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        fortuneDto = _mapper.Map<FortuneOfDayDto>(fortune.f);
        SetFortuneOfDayCocke(fortune.Culture, fortuneDto);

        return fortuneDto;
    }

    private string GetFortuneOfDayKey(string language) => $"fortuneOfDay{language}";
    private FortuneOfDayDto? GetFortuneOfDayCocke(string language) => _cache.Get<FortuneOfDayDto>(GetFortuneOfDayKey(language));
    private void SetFortuneOfDayCocke(string language, FortuneOfDayDto fortune) => _cache.Set(GetFortuneOfDayKey(language), fortune, TimeSpan.FromHours(3));
}
