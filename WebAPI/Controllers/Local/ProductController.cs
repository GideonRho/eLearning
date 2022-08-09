using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models.API.Requests;
using WebAPI.Misc;
using WebAPI.Models.Database.Enums;
using WebAPI.Services;

namespace WebAPI.Controllers.Local
{
    [ApiController]
    [LocalHostFilter]
    [Route("local/product")]
    public class ProductController : ControllerBase
    {

        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<List<string>>> GenerateKeys(GenerateKeysPayload payload)
        {
            EProductKeyType type = (EProductKeyType)payload.Type;

            if (type == EProductKeyType.Duration)
                return await _productService.GenerateKeys(payload.Amount, payload.Duration);
            if (type == EProductKeyType.Date)
                return await _productService.GenerateKeys(payload.Amount, 
                    new DateTime(payload.Year, payload.Month, payload.Day));

            return BadRequest();
        }

    }
}