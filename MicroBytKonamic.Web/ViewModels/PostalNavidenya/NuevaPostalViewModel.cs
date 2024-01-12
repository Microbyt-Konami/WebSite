using System.ComponentModel.DataAnnotations;

namespace MicroBytKonamic.Web.ViewModels.PostalNavidenya;

public class NuevaPostalViewModel
{
    [Required] public DateTime? Fecha { get; set; }
    [Required, StringLength(50)] public string? Nick { get; set; }
    [Required] public string? Felicitacion { get; set; }
}
