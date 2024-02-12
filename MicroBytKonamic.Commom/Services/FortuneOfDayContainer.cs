using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Services;

public class FortuneOfDayContainer
{
    public DateTime? Day { get; set; }
    public ImmutableDictionary<string, string>? LanguagesFortunes { get; set; }

    public string? GetFortuneOfDayByLang(string lang)
        => (LanguagesFortunes == null)
            ? null
            : LanguagesFortunes.TryGetValue(lang, out var fortune)
                ? fortune
                : LanguagesFortunes.Values.FirstOrDefault();
}
