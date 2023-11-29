using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

public class GetFelicitacionResult
{
    public required FelicitacionDto FelicitacionDto { get; set; }
    public required IntegerIntervals Intervals { get; set; }
}
