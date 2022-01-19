using Business.Cooperative.BusinessModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooperative.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoopController : ControllerBase
    {
        // GET: api/<CoopController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CoopController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CoopController>
        [HttpPost("Create")]
        [SwaggerResponse(200, "Create", typeof(Coop))]
        public void Post([FromBody] Coop cooperative)
        {
        }

        // PUT api/<CoopController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoopController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}