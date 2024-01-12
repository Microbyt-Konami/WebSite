using MicroBytKonamic.Commom.Exceptions;
using MicroBytKonamic.Commom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MicroBytKonamic.Web.Controllers.api
{
    [Route("api/postales")]
    [ApiController]
    public class PostalesController : ControllerBase
    {
        private readonly IPostalesServices _postalesServices;

        public PostalesController(IPostalesServices postalesServices)
        {
            _postalesServices = postalesServices;
        }

        [HttpPost("getfelicitacion")]
        public async Task<ActionResult<GetFelicitacionResult>> GetFelicitacion(GetFelicitacionIn input)
        //=> _postalesServices.GetFelicitacionAsync(input);
        {
            var result = await _postalesServices.GetFelicitacionAsync(input);

            return result.FelicitacionDto != null ? result : NoContent();
        }

        [HttpPost("altafelicitacion")]
        public async Task<IntegerIntervals> AltaFelicitacion(AltaFelicitacionIn input)
        // => _postalesServices.AltaFelicitacion(input);
        {
            try
            {
                return await _postalesServices.AltaFelicitacionAsync(input);
            }
            catch (MBException) { throw; }
            catch (Exception ex) { throw new SuportCallApiException(ex); }
        }

        [HttpGet("/PostalNavidenya/MusicaNavidadMP3")]
        public async Task<IActionResult> MusicaNavidadMP3()
        {
            var data = await _postalesServices.ReadMP3NavidadAsync();

            return File(data, "audio/mpeg");
        }
    }
}
