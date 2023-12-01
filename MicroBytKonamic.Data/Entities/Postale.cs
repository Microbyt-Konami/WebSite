using System;
using System.Collections.Generic;

namespace MicroBytKonamic.Data.Entities;

public partial class Postale
{
    public int IdPostales { get; set; }

    public int? Anyo { get; set; }

    public string? Nick { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Texto { get; set; }
}
