using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Interfaces;

public interface ILanguagesServices
{
    Task<LanguageDto[]> GetSupportedLanguageDtos(CancellationToken cancellationToken);
}
