using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

public class ImportFortunesIn
{
    public required string Filename { get; set; }
    public required string Language { get; set; }
    public required string Topic { get; set; }
    public required string[] Fortunes { get; set; }
}
