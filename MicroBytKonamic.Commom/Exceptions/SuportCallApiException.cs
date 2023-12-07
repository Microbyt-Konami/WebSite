using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Exceptions;

public class SuportCallApiException : Exception
{
    public SuportCallApiException() : base("No se ha podido realizar la operación. Intentelo más tarde") { }
    public SuportCallApiException(Exception inner) : base("No se ha podido realizar la operación. Intentelo más tarde", inner) { }
}
