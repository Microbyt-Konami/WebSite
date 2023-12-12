using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Exceptions;

public class MBException : ApplicationException
{
    public MBException()
    {
    }

    public MBException(string? message) : base(message)
    {
    }

    public MBException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
