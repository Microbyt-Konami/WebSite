using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Commom.Dto;

public class FelicitacionDto
{
    [Required] public string? Nick { get; set; }
    [Required] public JsonDateTime? Fecha { get; set; }

    [Required] public string? Texto { get; set; }
}
