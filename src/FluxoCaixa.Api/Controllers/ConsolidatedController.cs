using Microsoft.AspNetCore.Mvc;
using FluxoCaixa.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace FluxoCaixa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsolidatedController : ControllerBase
    {
        private readonly ITransactionService _service;

        public ConsolidatedController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] decimal initialBalance = 0)
        {
            if (from > to) return BadRequest("'from' must be less or equal to 'to'.");
            var list = await _service.GetConsolidatedAsync(from, to, initialBalance);
            return Ok(list);
        }
    }
}