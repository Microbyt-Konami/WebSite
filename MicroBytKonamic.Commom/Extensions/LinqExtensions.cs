using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq;

public static class LinqExtensions
{
    public static int GetHashCodeEx<T>(this IEnumerable<T> col)
    {
        var hc = 0;

        foreach (var p in col)
        {
            hc ^= p.GetHashCode();
            hc = (hc << 7) | (hc >> (32 - 7)); //rotale hc to the left to swipe over all bits
        }
        return hc;
    }
}
