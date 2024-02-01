using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Tools.Fortunes.Data;

[DebuggerDisplay("Filename: {Filename} Language: {Language} Topic: {Topic}")]
internal class Files
{
    public required string Filename { get; set; }
    public string? Language { get; set; }
    public string? Topic { get; set; }
    public bool? Skip { get; set; }
}
