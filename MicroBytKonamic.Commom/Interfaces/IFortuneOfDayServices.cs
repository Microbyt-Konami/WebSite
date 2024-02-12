using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Interfaces;

public interface IFortuneOfDayServices
{
    Task LoadFortuneOfDayIntoContainerAsync(DateTime day, int? maxCharSize, CancellationToken cancellationToken);
    Task<bool> LoadFortuneOfDayIntoContainerFromBDAsync(DateTime day, CancellationToken cancellationToken);
    Task AddFortuneOfDayIntoContainerFromBDAsync(DateTime day, int? maxCharSize, CancellationToken cancellationToken);
    void SetFortunesOfDay(DateTime day, ImmutableDictionary<string, string> langsFortunes);
}
