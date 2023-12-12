using System.ComponentModel.DataAnnotations;

namespace MicroBytKonamic.Web.ViewModels.PostalNavidenya;

public class NuevaPostalViewModel
{
    public required DateTime Fecha { get; set; }
    [StringLength(50)] public required string Nick { get; set; }
    public required string Felicitacion { get; set; }
}
