using Business.Cooperative;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooperative.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
       

        // POST api/<ProductionPlan>
        [HttpPost("projection")]
        [SwaggerResponse(200, "projection", typeof(ProjectionPerPeriod))]
        public Projection Post([FromBody] ProjectionPerPeriod prjperiod)
        {
            ProjectionPerDuration prjdur = ProcessCreator.GetProjectionPerDuration();
            return prjdur.GetProjectionPerPeriod(prjperiod);
        }
    }
}
