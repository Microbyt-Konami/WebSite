using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Interfaces;

public interface IPostalesServices
{
    int CalcAnyo(DateTime date);
    Task<GetFelicitacionResult> GetFelicitacionAsync(GetFelicitacionIn input);
    Task<IntegerIntervals> AltaFelicitacionAsync(AltaFelicitacionIn input, CancellationToken cancellationToken = default);
    Task<byte[]> ReadMP3NavidadAsync();
}
