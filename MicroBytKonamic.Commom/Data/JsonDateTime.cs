using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Data;

public struct JsonDateTime
{
    private DateTime _date;

    // in utc
    public long Ticks
    {
        get => _date.Ticks;
        set
        {
            _date = new DateTime(value, DateTimeKind.Utc);
        }
    }

    public JsonDateTime(DateTime dt) => _date = dt.ToUniversalTime();

    public static implicit operator DateTime(JsonDateTime jdt) => jdt._date.ToLocalTime();
    public static implicit operator JsonDateTime(DateTime dt) => new JsonDateTime(dt);
}
