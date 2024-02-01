using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroBytKonamic.Tools.Fortunes.Services;

internal class JsonFortunesServices
{
    private readonly IHostEnvironment _env;
    private readonly ILogger<JsonFortunesServices> _log;

    public JsonFortunesServices(IHostEnvironment env, ILogger<JsonFortunesServices> log)
    {
        _env = env;
        _log = log;
    }

    public async Task<JsonFortunes?> ReadJsonFortunesAsync()
    {
        using var stream = new FileStream(Path.Combine(_env.ContentRootPath, "fortunes", "fortunes.json"), FileMode.Open, FileAccess.Read, FileShare.Read);

        return await JsonSerializer.DeserializeAsync<JsonFortunes>(stream!, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public async Task<ImportFortunesIn[]> ProcessJsonFortunes(JsonFortunes json, CancellationToken cancellationToken)
    {
        var result = new List<ImportFortunesIn>();

        foreach (var file in json.Files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fortunes = await ProcessFile(file);

            if (fortunes != null)
                result.Add(fortunes);
        }

        return result.ToArray();
    }

    private async Task<ImportFortunesIn?> ProcessFile(Files file)
    {
        _log.LogInformation($"Process {file.Filename}");
        if (!string.IsNullOrWhiteSpace(file.Language))
            _log.LogInformation($"Language: {file.Language}");
        if (!string.IsNullOrWhiteSpace(file.Topic))
            _log.LogInformation($"Topic: {file.Topic}");
        DefaultPropFile(file);

        var fortunes = await ReadFortunes(file);

        if (fortunes == null)
            return null;

        _log.LogInformation($"Count fortunes: {fortunes.Length}");

        return new ImportFortunesIn
        {
            Filename = file.Filename,
            Language = file.Language!,
            Topic = file.Topic!,
            Fortunes = fortunes
        };
    }

    private void DefaultPropFile(Files file)
    {
        // Default en
        if (!string.IsNullOrWhiteSpace(file.Language))
        {
            file.Language = "en";
            _log.LogWarning($"default language: {file.Language}");
        }

        // Default filename until first dot. Example => filename: murphy.fortunes.u8 => topic: murphy
        if (!string.IsNullOrWhiteSpace(file.Topic))
        {
            int dotIdx = file.Filename.IndexOf('.');

            file.Topic = (dotIdx > 0) ? file.Filename.Substring(0, dotIdx - 1) : file.Filename;
            _log.LogWarning($"default topic: {file.Topic}");
        }
    }

    private async Task<string[]?> ReadFortunes(Files file)
    {
        var filePath = Path.Combine(_env.ContentRootPath, "fortunes", file.Filename);

        if (!File.Exists(filePath))
        {
            _log.LogWarning($"{file.Filename} does not exist.");

            return null;
        }

        var text = await File.ReadAllTextAsync(filePath);
        var fortunes = text.Split('%', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();

        return fortunes;
    }
}
