﻿using MicroBytKonamic.Commom.Exceptions;
using MicroBytKonamic.Commom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroBytKonamic.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostalesController : ControllerBase
    {
        private readonly IPostalesServices _postalesServices;

        public PostalesController(IPostalesServices postalesServices)
        {
            _postalesServices = postalesServices;
        }

        [HttpPost("getfelicitacion")]
        public Task<GetFelicitacionResult> GetFelicitacion(GetFelicitacionIn input)
            => _postalesServices.GetFelicitacion(input);

        [HttpPost("altafelicitacion")]
        public async Task<IntegerIntervals> AltaFelicitacion(AltaFelicitacionIn input)
        // => _postalesServices.AltaFelicitacion(input);
        {
            try
            {
                return await _postalesServices.AltaFelicitacion(input);
            }
            catch (MBException) { throw; }
            catch (Exception ex) { throw new SuportCallApiException(ex); }
        }
    }
}
