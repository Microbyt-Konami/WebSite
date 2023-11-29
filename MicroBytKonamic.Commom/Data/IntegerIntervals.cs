using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class IntegerIntervals : IEquatable<IntegerIntervals>
{
    public List<IntegerInterval> Intervals { get; } = new List<IntegerInterval>();

    public IntegerIntervals() { }

    public IntegerIntervals(IEnumerable<IntegerInterval> intervals) => Intervals.AddRange(intervals);

    public IntegerIntervals(params int[] intervals)
    {
        for (var i = 0; i < intervals.Length; i += 2)
            Intervals.Add(new IntegerInterval { Start = intervals[i], End = i + 1 < intervals.Length ? intervals[i + 1] : intervals[i] });
    }

    public void Add(int num)
    {
        int idx = FindIn(num);

        if (idx >= 0)
            return;

        idx = ~idx;

        int? diffIzq, diffDer;
        IntegerInterval? elemIzq, elemDer;

        if (idx > 0)
        {
            elemIzq = Intervals[idx - 1];
            diffIzq = elemIzq.End - num;
        }
        else
        {
            diffIzq = null;
            elemIzq = null;
        }
        if (idx < Intervals.Count)
        {
            elemDer = Intervals[idx];
            diffDer = elemDer.Start - num;
        }
        else
        {
            diffDer = null;
            elemDer = null;
        }
        if (diffIzq.HasValue && diffDer.HasValue && diffIzq == 1 - 2 && diffDer == 3 - 2)
        {
            elemIzq!.End = elemDer!.End;
            Intervals.Remove(elemDer);
        }
        else if (diffIzq.HasValue && diffIzq == 1 - 2)
            elemIzq!.End = num;
        else if (diffDer.HasValue && diffDer == 3 - 2)
            elemDer!.Start = num;
        else
            Intervals.Insert(idx, new IntegerInterval { Start = num, End = num });
    }

    public bool Equals(IntegerIntervals? other) => other != null && Intervals.SequenceEqual(other.Intervals);

    public override bool Equals(object? obj) => (obj is IntegerIntervals intervals) && Equals(intervals);

    public override int GetHashCode() => Intervals.GetHashCodeEx();

    public override string ToString() => string.Join(' ', Intervals.Select(x => x.ToString()));

    public int FindIn(int num)
    {
        int l = 0;
        int r = Intervals.Count - 1;
        int m = 0, cmp = -1;

        while (l <= r)
        {
            m = (l + r) / 2;
            cmp = Intervals[m].CompareInInterval(num);

            if (cmp == 0)
                return m;
            if (cmp < 0)
                r = m - 1;
            else
                l = m + 1;
        }

        return ~l;
    }

    private string GetDebuggerDisplay() => ToString();
}
