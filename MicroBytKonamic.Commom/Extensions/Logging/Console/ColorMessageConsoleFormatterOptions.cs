using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.Console;

public class ColorMessageConsoleFormatterOptions : ConsoleFormatterOptions
{
    /// <summary>
    /// Determines when to use color when logging messages.
    /// </summary>
    public LoggerColorBehavior ColorBehavior { get; set; }
}
