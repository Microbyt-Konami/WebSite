using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class FortunesServices(MicrobytkonamicContext _dbContext, IRandomServices _random, IMapper _mapper) : IFortunesServices
{
    private readonly MicrobytkonamicContext _dbContext = _dbContext;
    private readonly IRandomServices _random = _random;
    private readonly IMapper _mapper = _mapper;

    public async Task<FortuneOfDayDto?> GetIdFortuneOfDayAsync(string language, int? maxCharSize = null, CancellationToken cancellationToken = default)
    {
        var fortunes =
        (
            from f in _dbContext.Fortunes
            where !maxCharSize.HasValue || f.Fortune1.Length <= maxCharSize.Value
            join ff in _dbContext.Filesfortunes on f.IdFilesFortunes equals ff.IdFilesFortunes
            join l in _dbContext.Languages on ff.IdLanguages equals l.IdLanguages
            where l.Culture == language
            select f
        );
        var count = fortunes.Count();

        if (count == 0)
            return null;

        var idx = _random.NetInt(count);
        var fortune = await fortunes.Skip(idx).Take(1).FirstAsync(cancellationToken);
        var fortuneDto = _mapper.Map<FortuneOfDayDto>(fortune);

        cancellationToken.ThrowIfCancellationRequested();

        return fortuneDto;
    }
}
