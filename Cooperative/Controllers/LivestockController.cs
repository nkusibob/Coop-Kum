using Business.Cooperative.Api;
using Business.Cooperative.Api.RequestModel;
using Business.Cooperative.BusinessModel;
using Microsoft.AspNetCore.Mvc;
using Model.Cooperative;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<List<Goat>>> GetAvailableGoats(int coopId) 
        {
            var availableGoats =await  _farm.ListAvailableLivestock(coopId);
            return Ok(availableGoats);
        }
        [HttpGet("details/{livestockId}")]
        public async Task<ActionResult<Goat>> UpdateGoatDetails(int livestockId)
        {
            var detailedGoat = await _farm.UpdateDetails(livestockId);
            return Ok(detailedGoat);
        }

        [HttpPost("details/{livestockId}")]
        public async Task<ActionResult<Goat>> UpdateGoatDetails(int livestockId, [FromBody][Bind("LivestockId, LivestockType, Name, Age, IsSold, Price, IsPregnant, LastDropped, NumFemalesPaired, CoopId")] Goat goat)
        {
            var updatedGoat = await _farm.UpdateDetails(livestockId, goat);
            return Ok(updatedGoat);
        }

        [HttpPost("breed")]
        public async Task<IActionResult> BreedGoatsAsync(int idCoop) // Change the method name to BreedGoats
        {
            var eligibleCouples = await _farm.BreedLivestockAsync(idCoop);
            
            return Ok(eligibleCouples);
        }

        [HttpPost("naming")]
        public async Task<IActionResult> NamingLivestockAsync(int idCoop, NamingLivestockRequest request)
        {
            var allNewBorn = await _farm.HandleBirth(idCoop, request.KidGenders, request.KidNames);
            // Perform the necessary logic to obtain the user input for naming the goats
            // You can use the returned data to update the newly born goats with their names or store the names for further processing

            // Example logic:
            // var goatNames = new List<string>();
            // foreach (var kid in kids)
            // {
            //     // Obtain the name for each goat, e.g., through user input or generating names programmatically
            //     // goatNames.Add(obtainedName);
            //     // Update the goat's name, e.g., kid.Name = obtainedName;
            // }

            // Return the goat names or any other relevant information
            // return Ok(goatNames);
            return Ok(allNewBorn);
        }
       
        
        [HttpPost("buy")]
        public async Task<IActionResult> BuyGoat(Goat goat)
        {
            var result =await  _farm.BuyLivestock(goat);
            return Ok(result);
        }

        [HttpPost("eat")]
        public async Task<IActionResult> EatGoatAsync(string eatenGoatName,int idCoop) // Change the method name to EatGoat
        {
            var eatenGoat =await _farm.EatLivestock(eatenGoatName, idCoop);
            return Ok(eatenGoat);
        }

        [HttpPost("optimize")]
        public async Task<IActionResult> OptimizeHerdAsync(bool extendGenetics, int malesToKeep, bool sellGoats, decimal sellPrice, string goatName,int idCoop) // Change the method name to OptimizeHerd
        {
            var result =await _farm.OptimizeHerdGrowthAsync(extendGenetics, malesToKeep, sellGoats, sellPrice, goatName, idCoop);
            return Ok(result);
        }
    }
}
