using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Services;

public class FortuneOfDayServicesContainer
{
    public DateTime? Day { get; set; }
    public Dictionary<string, FortuneOfDayDto>? LanguagesFortunes { get; set; }
}
