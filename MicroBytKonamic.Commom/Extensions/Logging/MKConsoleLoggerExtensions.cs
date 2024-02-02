using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    public static class MKConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddColorMessageConsole(this ILoggingBuilder builder)
            => builder
                .AddConsole(options => options.FormatterName = "colorMessage")
                .AddConsoleFormatter<ColorMessageConsoleFormatter, ColorMessageConsoleFormatterOptions>();
        public static ILoggingBuilder AddColorMessageConsole(this ILoggingBuilder builder, Action<ColorMessageConsoleFormatterOptions> configure)
            => builder
                .AddConsole(options => options.FormatterName = "colorMessage")
                .AddConsoleFormatter<ColorMessageConsoleFormatter, ColorMessageConsoleFormatterOptions>(configure);
    }
}
