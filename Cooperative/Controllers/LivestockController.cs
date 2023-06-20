using Business.Cooperative.Api;
using Business.Cooperative.BusinessModel;
using Microsoft.AspNetCore.Mvc;
using Model.Cooperative;
using System.Collections.Generic;

namespace Cooperative.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class LivestockController : ControllerBase
    {
        private readonly CooperativeContext _context;
        private readonly IFarm<Goat> _farm; // Use IFarm<Goat> instead of IFarm<Livestock>

        public LivestockController(CooperativeContext context, GoatFarm farm) // Use GoatFarm as the implementation
        {
            _context = context;
            _farm = farm;
        }

        // Existing actions

        [HttpGet("available/{coopId}")]
        public ActionResult<List<Goat>> GetAvailableGoats(int coopId) 
        {
            var availableGoats = _farm.ListAvailableLivestock(coopId);
            return Ok(availableGoats);
        }

        [HttpPost("breed")]
        public IActionResult BreedGoats(int kidCount, LivestockGender kidGender, string kidName,int idCoop) // Change the method name to BreedGoats
        {
            _farm.BreedLivestock(kidCount, kidGender, kidName,idCoop);
            return Ok();
        }

        [HttpPost("buy")]
        public IActionResult BuyGoat(string name, string genderInput, string input, double price, [FromQuery] int idCoop, [FromQuery] double totalPrice)
        {
            var result = _farm.BuyLivestock(name, genderInput, input, price, idCoop, ref totalPrice);
            return Ok(result);
        }

        [HttpPost("eat")]
        public IActionResult EatGoat(string eatenGoatName) // Change the method name to EatGoat
        {
            _farm.EatLivestock(eatenGoatName);
            return Ok();
        }

        [HttpPost("optimize")]
        public IActionResult OptimizeHerd(bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice, string goatName) // Change the method name to OptimizeHerd
        {
            var result = _farm.OptimizeHerdGrowth(extendGenetics, malesToKeep, sellGoats, sellPrice, goatName);
            return Ok(result);
        }
    }
}
