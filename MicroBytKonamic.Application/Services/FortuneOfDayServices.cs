using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class FortuneOfDayServices(LanguagesContainer _languagesContainer, FortuneOfDayContainer _container, MicrobytkonamicContext _dbContext, IFortunesServices _fortunesServices, ILanguagesServices _languagesServices)
    : IFortuneOfDayServices
{
    private readonly LanguagesContainer _languagesContainer = _languagesContainer;
    private readonly FortuneOfDayContainer _container = _container;
    private readonly MicrobytkonamicContext _dbContext = _dbContext;
    private readonly IFortunesServices _fortunesServices = _fortunesServices;
    private readonly ILanguagesServices _languagesServices = _languagesServices;

    public async Task LoadFortuneOfDayIntoContainerAsync(DateTime day, CancellationToken cancellationToken)
    {
        if (_container.Day.HasValue && _container.Day.Value.Date == day.Date)
            return;

        if (await LoadFortuneOfDayIntoContainerFromBDAsync(day, cancellationToken))
            return;

        cancellationToken.ThrowIfCancellationRequested();

        await AddFortuneOfDayIntoContainerFromBDAsync(day, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public void SetFortunesOfDay(DateTime day, ImmutableDictionary<string, string> langsFortunes)
    {
        _container.Day = day;
        _container.LanguagesFortunes = langsFortunes;
    }

    public async Task<bool> LoadFortuneOfDayIntoContainerFromBDAsync(DateTime day, CancellationToken cancellationToken)
    {
        var _day = day.Date;
        var query =
            from fd in _dbContext.Fortunesofdays
            where fd.Day == _day
            select fd;

        if (query.Take(1).Count() == 0)
            return false;

        var fortunes = await
        (
            from fd in query
            join f in _dbContext.Fortunes on fd.IdFortunes equals f.IdFortunes
            join ff in _dbContext.Filesfortunes on f.IdFilesFortunes equals ff.IdFilesFortunes
            join l in _dbContext.Languages on ff.IdLanguages equals l.IdLanguages
            select new { l.Culture, f.Fortune1 }
        ).ToDictionaryAsync(f => f.Culture, f => f.Fortune1, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        SetFortunesOfDay(day, fortunes.ToImmutableDictionary());

        return true;
    }

    public async Task AddFortuneOfDayIntoContainerFromBDAsync(DateTime day, CancellationToken cancellationToken)
    {
        var languages = await _languagesServices.GetSupportedLanguageDtos(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        var fortunes = new Dictionary<string, string>();

        foreach (var lang in languages)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fortune = await _fortunesServices.GetIdFortuneOfDayAsync(lang.Culture, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            if (fortune != null)
            {
                fortunes.Add(lang.Culture, fortune.Fortune1);
                await _dbContext.Fortunesofdays.AddAsync(new Fortunesofday
                {
                    Day = day.Date,
                    IdLanguages = lang.IdLanguages,
                    IdFortunes = fortune.IdFortunes
                });
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        SetFortunesOfDay(day, fortunes.ToImmutableDictionary());
    }
}
