﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MicroBytKonamic.Data.Entities;

public partial class Language
{
    public int IdLanguages { get; set; }

    public string Culture { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Filesfortune> Filesfortunes { get; set; } = new List<Filesfortune>();
}