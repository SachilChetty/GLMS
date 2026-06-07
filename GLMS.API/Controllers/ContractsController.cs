using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GLMS.API.Models;
using GLMS.API.Services;

namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // JWT required on all endpoints
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _service;

        public ContractsController(IContractService service)
        {
            _service = service;
        }

        // GET /api/contracts?status=Active
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            var contracts = await _service.GetContractsAsync(status);
            return Ok(contracts);
        }

        // GET /api/contracts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _service.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return Ok(contract);
        }

        // POST /api/contracts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContractDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateContractAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH /api/contracts/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateContractStatusDto dto)
        {
            try
            {
                var updated = await _service.UpdateContractStatusAsync(id, dto.Status);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
