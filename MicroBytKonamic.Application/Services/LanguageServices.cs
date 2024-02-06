using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class LanguageServices : ILanguageServices
{
    private readonly string[] _supportedLanguages = new[] { "es", "en" };
    private readonly string _defaultLanguage = "en";

    public string[] SupportedLanguages => _supportedLanguages;

    public string DefaultLanguage => _defaultLanguage;
}
