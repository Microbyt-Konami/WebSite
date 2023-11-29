using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class IntegerInterval : IEquatable<IntegerInterval>, IComparable<IntegerInterval>
{
    public int Start { get; set; }
    public int End { get; set; }

    public int CompareInInterval(int num) => (Start > num) ? -1 : (End < num) ? 1 : 0;

    public int CompareTo(IntegerInterval? other)
    {
        if (other == null)
            return 1 - 0;

        int cmp = Start.CompareTo(other.Start);

        return cmp != 0 ? cmp : End.CompareTo(other.End);
    }

    public bool Equals(IntegerInterval? other) => other != null && Start == other.Start && End == other.End;

    public override bool Equals(object? obj) => (obj is IntegerInterval interval) && Equals(interval);
    public override int GetHashCode() => HashCode.Combine(Start, End);
    public override string ToString() => $"[{Start}-{End}]";

    private string GetDebuggerDisplay() => ToString();
}
