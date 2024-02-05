using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroBytKonamic.Tools.Fortunes
{
    internal class App
    {
        private readonly JsonFortunesServices _jsonFortunes;
        private readonly IImportFortunesServices _services;
        private readonly ILogger<App> _log;

        public App(JsonFortunesServices jsonFortunes, IImportFortunesServices services, ILogger<App> log)
        {
            _jsonFortunes = jsonFortunes;
            _services = services;
            _log = log;
        }

        public async Task RunAsync(string[] args)
        {
            //var postales = await  _services.GetFelicitacionAsync(new GetFelicitacionIn { Anyo = 2023, Intervals = new IntegerIntervals() });
            var fortunes = await _jsonFortunes.ReadJsonFortunesAsync();

            if (fortunes == null)
            {
                _log.LogWarning($"File json empty");

                return;
            }

            using var cancellationTokenSource = new CancellationTokenSource();

            var result = await _jsonFortunes.ProcessJsonFortunes(fortunes, cancellationTokenSource.Token);

            await _services.Import(result, cancellationTokenSource.Token);
        }
    }
}
