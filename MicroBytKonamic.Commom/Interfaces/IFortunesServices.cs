using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Interfaces;

public interface IFortunesServices
{
    Task<FortuneOfDayDto> GetFortuneOfDayAsync(string language, CancellationToken cancellationToken);
}
