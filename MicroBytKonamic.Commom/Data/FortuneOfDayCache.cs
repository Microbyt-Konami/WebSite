using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

public class FortuneOfDayCache
{
    public required DateTime Day { get; set; }
    public required Dictionary<string, FortuneOfDayDto> LanguagesFortunes { get; set; }
}
