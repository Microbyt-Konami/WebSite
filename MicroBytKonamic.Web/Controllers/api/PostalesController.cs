﻿using MicroBytKonamic.Commom.Interfaces;
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

        [HttpGet("getfelicitacion")]
        public Task<FelicitacionDto> GetFelicitacion(IntegerIntervals[]? intervals)
            => _postalesServices.GetFelicitacion(intervals);
    }
}