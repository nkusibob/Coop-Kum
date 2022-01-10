using Business.Cooperative;
using Business.Cooperative.ProjectState;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
