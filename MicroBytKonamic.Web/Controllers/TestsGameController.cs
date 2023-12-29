using Microsoft.AspNetCore.Mvc;

namespace MicroBytKonamic.Web.Controllers
{
    public class TestsGameController : Controller
    {
        public IActionResult WebGl(string id)
        {
            return View("GameWebGL", id);
        }
    }
}
