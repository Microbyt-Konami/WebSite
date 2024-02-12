using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.Console;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/console-log-formatter?source=recommendations#set-formatter-with-configuration

public class ColorMessageConsoleFormatter : ConsoleFormatter, IDisposable
{
    const string DefaultForegroundColor = "\x1B[39m\x1B[22m";
    const string DefaultBackgroundColor = "\x1B[49m";

    private readonly IDisposable? _optionsReloadToken;
    private ColorMessageConsoleFormatterOptions _formatterOptions;

    public ColorMessageConsoleFormatter(IOptionsMonitor<ColorMessageConsoleFormatterOptions> options)
        // Case insensitive
        : base("colorMessage") =>
        (_optionsReloadToken, _formatterOptions) =
            (options.OnChange(ReloadLoggerOptions), options.CurrentValue);

    private void ReloadLoggerOptions(ColorMessageConsoleFormatterOptions options) =>
        _formatterOptions = options;

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        string? message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);

        if (message is null)
        {
            return;
        }

        LogLevel logLevel = logEntry.LogLevel;
        ConsoleColors logLevelColors = GetLogLevelConsoleColors(logLevel);

        //textWriter.WriteColoredMessage(message, logLevelColors.Background, logLevelColors.Foreground);
        //textWriter?.WriteLine(message);
        if (logLevelColors.Background.HasValue)
            textWriter?.Write(GetBackgroundColorEscapeCode(logLevelColors.Background.Value));
        if (logLevelColors.Foreground.HasValue)
            textWriter?.Write(GetBackgroundColorEscapeCode(logLevelColors.Foreground.Value));

        textWriter?.WriteLine(message);
    }

    public void Dispose() => _optionsReloadToken?.Dispose();

    private ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel)
    {
        // We shouldn't be outputting color codes for Android/Apple mobile platforms,
        // they have no shell (adb shell is not meant for running apps) and all the output gets redirected to some log file.
        bool disableColors = (_formatterOptions.ColorBehavior == LoggerColorBehavior.Disabled) || (_formatterOptions.ColorBehavior == LoggerColorBehavior.Default);

        if (disableColors)
        {
            return new ConsoleColors(null, null);
        }
        // We must explicitly set the background color if we are setting the foreground color,
        // since just setting one can look bad on the users console.
        return logLevel switch
        {
            LogLevel.Trace => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
            LogLevel.Debug => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
            LogLevel.Information => new ConsoleColors(ConsoleColor.DarkGreen, ConsoleColor.Black),
            LogLevel.Warning => new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black),
            LogLevel.Error => new ConsoleColors(ConsoleColor.Black, ConsoleColor.DarkRed),
            LogLevel.Critical => new ConsoleColors(ConsoleColor.White, ConsoleColor.DarkRed),
            _ => new ConsoleColors(null, null)
        };
    }

    static string GetForegroundColorEscapeCode(ConsoleColor color) =>
        color switch
        {
            ConsoleColor.Black => "\x1B[30m",
            ConsoleColor.DarkRed => "\x1B[31m",
            ConsoleColor.DarkGreen => "\x1B[32m",
            ConsoleColor.DarkYellow => "\x1B[33m",
            ConsoleColor.DarkBlue => "\x1B[34m",
            ConsoleColor.DarkMagenta => "\x1B[35m",
            ConsoleColor.DarkCyan => "\x1B[36m",
            ConsoleColor.Gray => "\x1B[37m",
            ConsoleColor.Red => "\x1B[1m\x1B[31m",
            ConsoleColor.Green => "\x1B[1m\x1B[32m",
            ConsoleColor.Yellow => "\x1B[1m\x1B[33m",
            ConsoleColor.Blue => "\x1B[1m\x1B[34m",
            ConsoleColor.Magenta => "\x1B[1m\x1B[35m",
            ConsoleColor.Cyan => "\x1B[1m\x1B[36m",
            ConsoleColor.White => "\x1B[1m\x1B[37m",

            _ => DefaultForegroundColor
        };

    static string GetBackgroundColorEscapeCode(ConsoleColor color) =>
        color switch
        {
            ConsoleColor.Black => "\x1B[40m",
            ConsoleColor.DarkRed => "\x1B[41m",
            ConsoleColor.DarkGreen => "\x1B[42m",
            ConsoleColor.DarkYellow => "\x1B[43m",
            ConsoleColor.DarkBlue => "\x1B[44m",
            ConsoleColor.DarkMagenta => "\x1B[45m",
            ConsoleColor.DarkCyan => "\x1B[46m",
            ConsoleColor.Gray => "\x1B[47m",

            _ => DefaultBackgroundColor
        };

    private readonly struct ConsoleColors
    {
        public ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
        {
            Foreground = foreground;
            Background = background;
        }

        public ConsoleColor? Foreground { get; }

        public ConsoleColor? Background { get; }
    }
}
