using MicroBytKonamic.Commom.Interfaces;
using MicroBytKonamic.Web.ViewModels.PostalNavidenya;
using Microsoft.AspNetCore.Mvc;

namespace MicroBytKonamic.Web.Controllers;

public class PostalNavidenya : Controller
{
    private readonly IPostalesServices _postalesServices;

    public PostalNavidenya(IPostalesServices postalesServices)
    {
        _postalesServices = postalesServices;
    }

    public IActionResult Index(NuevaPostalViewModel vm)
    {
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> NuevaFelicitacion(NuevaPostalViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), vm);
        }

        var input = new AltaFelicitacionIn
        {
            Anyo = _postalesServices.CalcAnyo(DateTime.Now),
            FelicitacionDto = new FelicitacionDto
            {
                Fecha = vm.Fecha,
                Nick = vm.Nick,
                Texto = vm.Felicitacion
            }
        };

        await _postalesServices.AltaFelicitacionAsync(input);

        return View(nameof(VerPostalNavidenya));
    }

    public IActionResult VerPostalNavidenya() => View();

    public async Task<IActionResult> MusicaNavidadMP3()
    {
        var data = await _postalesServices.ReadMP3NavidadAsync();

        return File(data, "audio/mpeg");
    }
}
