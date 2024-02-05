using MicroBytKonamic.Commom.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class FortunesServices : IFortunesServices
{
    private readonly MicrobytkonamicContext _dbContext;
    private readonly ILogger<FortunesServices> _log;

    public FortunesServices(MicrobytkonamicContext dbContext, ILogger<FortunesServices> log)
    {
        _dbContext = dbContext;
        _log = log;
    }

    public async Task Import(ICollection<ImportFortunesIn> fortunes, CancellationToken cancellationToken)
    {
        var exs = new List<Exception>();
        int count = 0;

        foreach (var fortune in fortunes)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var rows = await ImportInternal(fortune, cancellationToken);

                if (rows.HasValue)
                {
                    count += rows.Value;

                    await _dbContext.SaveChangesAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                _dbContext.ChangeTracker.DetectChanges();
                throw;
            }
            catch (AggregateException ex)
            {
                _dbContext.ChangeTracker.DetectChanges();
                ex.Flatten().Handle(ex =>
                {
                    _log.LogError(ex.Message);
                    if (!(ex is MBException))
                        exs.Add(ex);

                    return true;
                });
            }
            catch (MBException ex)
            {
                _dbContext.ChangeTracker.DetectChanges();
                _log.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.DetectChanges();
                _log.LogError(ex.Message);
                exs.Add(ex);
            }
        }

        _log.LogInformation($"Se ha importado {count} fortunes");

        if (exs.Count > 0)
        {
            if (exs.Count == 1)
                throw exs[0];
            else
                throw new AggregateException(exs);
        }
    }

    private async Task<int?> ImportInternal(ImportFortunesIn fortuneImp, CancellationToken cancellationToken)
    {
        bool doUpdate = false;
        var language = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Culture == fortuneImp.Language, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (language == null)
            throw new MBException($"Language {fortuneImp.Language} don't exist");

        var topic = await _dbContext.Topicsfortunes.FirstOrDefaultAsync(t => t.Topic == fortuneImp.Topic);

        cancellationToken.ThrowIfCancellationRequested();

        if (topic == null)
        {
            topic = new Topicsfortune
            {
                Topic = fortuneImp.Topic,
            };

            await _dbContext.Topicsfortunes.AddAsync(topic, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            doUpdate = true;
        }

        var fortunesTxt = fortuneImp.Fortunes;
        var fortunes = await _dbContext.Filesfortunes.Include(f => f.Fortunes).FirstOrDefaultAsync(f => f.Filename == fortuneImp.Filename);
        ICollection<string>? fortunesIns = null;

        cancellationToken.ThrowIfCancellationRequested();

        if (fortunes == null)
        {
            fortunes = new Filesfortune
            {
                IdLanguagesNavigation = language,
                IdTopicsFortunesNavigation = topic,
                Filename = fortuneImp.Filename
            };

            await _dbContext.Filesfortunes.AddAsync(fortunes, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            fortunesIns = fortunesTxt;
            doUpdate = true;
        }
        else
        {
            if (fortunes.IdLanguagesNavigation != language)
                fortunes.IdLanguagesNavigation = language;
            if (fortunes.IdTopicsFortunesNavigation != topic)
                fortunes.IdTopicsFortunesNavigation = topic;
            //_dbContext.Fortunes.RemoveRange(fortunes.Fortunes);

            var query =
            (
                from f in fortunes.Fortunes
                join t in fortunesTxt on f.Fortune1 equals t into it
                from t in it.DefaultIfEmpty()
                select new { f, t }
            );
            var queryNoImport = query.Where(q => q.t != null);
            var queryDel =
            (
                from q in query
                where q.t == null
                select q.f
            ).ToArray();
            var queryIns =
            (
                from t in fortunesTxt
                join f in fortunes.Fortunes on t equals f.Fortune1 into _if
                from f in _if.DefaultIfEmpty()
                where f == null
                select t
            ).ToArray();
            var countNoImport = queryNoImport.Count();
            var countIns = queryIns.Length;
            var countDel = queryDel.Length;

            _log.LogInformation($"FileFortune have been imported. Fortunes equals: {countNoImport} to Ins: {countIns} to Del: {countDel}");
            if (countDel > 0)
            {
                _dbContext.Fortunes.RemoveRange(queryDel);
                doUpdate = true;
            }
            if (countIns > 0)
                fortunesIns = queryIns;
        }

        if (fortunesIns != null)
        {
            await _dbContext.Fortunes.AddRangeAsync
            (
                fortunesIns.Select
                (
                    t => new Fortune
                    {
                        IdFilesFortunesNavigation = fortunes,
                        Fortune1 = t
                    }
                ),
                cancellationToken
            );
            cancellationToken.ThrowIfCancellationRequested();

            doUpdate = true;
        }

        return (fortunesIns == null || fortunesIns.Count == 0) ? doUpdate ? 0 : null : fortunesIns.Count;
    }
}
