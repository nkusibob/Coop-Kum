using Business.Cooperative.Contracts.Coop;
using Business.Cooperative.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cooperative.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoopController : ControllerBase
    {
        private readonly ICoopService _coopService;

        public CoopController(ICoopService coopService)
        {
            _coopService = coopService;
        }

        [HttpGet]
        [SwaggerResponse(200, "Get all coops", typeof(IEnumerable<CoopDto>))]
        public async Task<ActionResult<IReadOnlyList<CoopDto>>> Get(CancellationToken cancellationToken)
        {
            var coops = await _coopService.GetAllAsync(cancellationToken);
            return Ok(coops);
        }

        [HttpGet("{id:int}")]
        [SwaggerResponse(200, "Get coop by id", typeof(CoopDto))]
        [SwaggerResponse(404, "Coop not found")]
        public async Task<ActionResult<CoopDto>> Get(int id, CancellationToken cancellationToken)
        {
            var coop = await _coopService.GetByIdAsync(id, cancellationToken);
            if (coop is null)
            {
                return NotFound();
            }

            return Ok(coop);
        }

        [HttpPost("Create")]
        [SwaggerResponse(201, "Create", typeof(CoopDto))]
        public async Task<ActionResult<CoopDto>> Post([FromBody] CreateCoopRequest request, CancellationToken cancellationToken)
        {
            var created = await _coopService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = created.IdCoop }, created);
        }

        [HttpPut("{id:int}")]
        [SwaggerResponse(200, "Updated", typeof(CoopDto))]
        [SwaggerResponse(404, "Coop not found")]
        public async Task<ActionResult<CoopDto>> Put(int id, [FromBody] UpdateCoopRequest request, CancellationToken cancellationToken)
        {
            var updated = await _coopService.UpdateAsync(id, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [SwaggerResponse(204, "Deleted")]
        [SwaggerResponse(404, "Coop not found")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _coopService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
