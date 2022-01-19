using Business.Cooperative;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cooperative.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationPlanController : ControllerBase
    {
        // POST api/<ProductionPlan>
        [HttpPost("Simulation")]
        [SwaggerResponse(200, "Simulation", typeof(Projection))]
        public Projection Post([FromBody] Goal goal)
        {
            return SimulationProcessor.GetSimulationForPeriod(goal);
        }
    }
}