using Microsoft.AspNetCore.Mvc;
using FluxoCaixa.Infrastructure.Services;
using FluxoCaixa.Core.DTOs;
using System;
using System.Threading.Tasks;

namespace FluxoCaixa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionDto dto)
        {
            try
            {
                if (dto.Amount <= 0) return BadRequest("Amount must be greater than zero.");
                var created = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(503, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var items = await _service.GetAsync(from, to);
            return Ok(items);
        }
    }
}