using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Services;

public class LanguagesContainer
{
    private readonly string[] _supportedLanguagesName = new[] { "es", "en" };
    private readonly string _defaultLanguage = "en";

    public string[] SupportedLanguagesName => _supportedLanguagesName;

    public string DefaultLanguage => _defaultLanguage;
}
