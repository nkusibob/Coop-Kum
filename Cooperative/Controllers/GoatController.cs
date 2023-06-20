//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Model.Cooperative;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Cooperative.Controllers
//{

//    [ApiController]
//    [Route("api/[controller]")]
//    public class GoatController : ControllerBase
//    {
//        private readonly CooperativeContext _context; // Replace with your DbContext class

//        public GoatController(CooperativeContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Goat
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Goat>>> GetGoats()
//        {
//            var goats = await _context.Goat.ToListAsync();
//            return goats;
//        }

//        // GET: api/Goat/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Goat>> GetGoat(int id)
//        {
//            var goat = await _context.Goat.FindAsync(id);

//            if (goat == null)
//            {
//                return NotFound();
//            }

//            return goat;
//        }

//        // POST: api/Goat
//        [HttpPost]
//        public async Task<ActionResult<Goat>> CreateGoat(Goat goat)
//        {
//            _context.Goat.Add(goat);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetGoat), new { id = goat.LivestockId }, goat);
//        }

//        // PUT: api/Goat/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateGoat(int id, Goat goat)
//        {
//            if (id != goat.LivestockId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(goat).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!GoatExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // DELETE: api/Goat/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteGoat(int id)
//        {
//            var goat = await _context.Goat.FindAsync(id);

//            if (goat == null)
//            {
//                return NotFound();
//            }

//            _context.Goat.Remove(goat);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool GoatExists(int id)
//        {
//            return _context.Goat.Any(e => e.LivestockId == id);
//        }
//    }

//}
