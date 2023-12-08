using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

public class AltaFelicitacionIn
{
    public required int Anyo { get; set; }
    public required FelicitacionDto FelicitacionDto { get; set; }
}
