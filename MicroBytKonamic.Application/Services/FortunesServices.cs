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
                count++;
                await ImportInternal(fortune, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (AggregateException ex)
            {
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
                _log.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                exs.Add(ex);
            }
        }

        if (exs.Count > 0)
        {
            _dbContext.ChangeTracker.DetectChanges();
            if (exs.Count == 1)
                throw exs[0];
            else
                throw new AggregateException(exs);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        _log.LogInformation($"Se ha importado {count} fortunes");
    }

    private async Task ImportInternal(ImportFortunesIn fortune, CancellationToken cancellationToken)
    {
        var language = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Culture == fortune.Language, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (language == null)
            throw new MBException($"Language {fortune.Language} don't exist");

        var topic = await _dbContext.Topicsfortunes.FirstOrDefaultAsync(t => t.Topic == fortune.Topic);

        cancellationToken.ThrowIfCancellationRequested();

        if (topic == null)
        {
            topic = new Topicsfortune
            {
                Topic = fortune.Topic,
            };

            await _dbContext.Topicsfortunes.AddAsync(topic, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
        }

        var fortunes = await _dbContext.Filesfortunes.FirstOrDefaultAsync(f => f.Filename == fortune.Filename);

        cancellationToken.ThrowIfCancellationRequested();

        if (fortunes == null)
        {
            fortunes = new Filesfortune
            {
                IdLanguagesNavigation = language,
                IdTopicsFortunesNavigation = topic,
                Filename = fortune.Filename
            };

            await _dbContext.Filesfortunes.AddAsync(fortunes, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
        }
        else
        {
            if (fortunes.IdLanguagesNavigation != language)
                fortunes.IdLanguagesNavigation = language;
            if (fortunes.IdTopicsFortunesNavigation != topic)
                fortunes.IdTopicsFortunesNavigation = topic;
            _dbContext.Fortunes.RemoveRange(fortunes.Fortunes);
        }

        await _dbContext.Fortunes.AddRangeAsync
        (
            fortune.Fortunes.Select
            (
                t => new Fortune
                {
                    IdFilesFortunesNavigation = fortunes,
                    Fortune1 = t
                }
            ),
            cancellationToken
        );
    }
}
