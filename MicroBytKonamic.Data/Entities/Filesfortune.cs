﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MicroBytKonamic.Data.Entities;

public partial class Filesfortune
{
    public int IdFilesFortunes { get; set; }

    public int IdTopicsFortunes { get; set; }

    public int IdLanguages { get; set; }

    public string Filename { get; set; }

    public virtual ICollection<Fortune> Fortunes { get; set; } = new List<Fortune>();

    public virtual Language IdLanguagesNavigation { get; set; }

    public virtual Topicsfortune IdTopicsFortunesNavigation { get; set; }
}