using Business.Cooperative.Api;
using Microsoft.AspNetCore.Mvc;
using Model.Cooperative;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Cooperation.Models.ViewModel;

namespace Web.Cooperation.Controllers
{
    public class LivestockController : Controller
    {
        private readonly IFarm<Goat> _apiGoatClient;

        public LivestockController(IFarm<Goat> apiGoatClient)
        {
            _apiGoatClient = apiGoatClient;
        }
        public async Task<IActionResult> BreedLivestock(int idCoop)
        {
            // Prompt the user for input if needed

            var eligiblePairsResult = await _apiGoatClient.BreedLivestockAsync(idCoop);
            var eligiblePairs = eligiblePairsResult.eligiblePairs;
            var pregnantGoats = eligiblePairsResult.pregnantGoats;
            var breedMessage = eligiblePairsResult.message;
            return View(eligiblePairsResult);
        }

        public async Task<IActionResult> ListAvailableLivestock(int idCoop)
        {
            var availableLivestock = await _apiGoatClient.ListAvailableLivestock(idCoop);

            return Json(availableLivestock);
        }

        public async Task<IActionResult> HandleBirth(int idCoop, List<LivestockGender> kidGenders, List<string> kidNames)
        {
            var birthResult = await _apiGoatClient.HandleBirth(idCoop, kidGenders, kidNames);
            var allNewBorn = birthResult.allNewBorn;
            var pregnantGoatsAfterBirth = birthResult.pregnantGoats;
            var birthMessage = birthResult.message;
            return View(birthResult);
            // Return a view or appropriate response with the results
        }

        public async Task<IActionResult> EatLivestock(int idCoop, string eatenGoatName)
        {


            var eatenGoat = await _apiGoatClient.EatLivestock(eatenGoatName, idCoop);

            return View(eatenGoat);
        }

        public async Task<IActionResult> OptimizeHerdGrowth(int idCoop, bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice, string goatName)
        {
            var optimizeResult = await _apiGoatClient.OptimizeHerdGrowthAsync(extendGenetics, malesToKeep, sellGoats, sellPrice, goatName, idCoop);
            var goatsToKeep = optimizeResult.goatsToKeep;
            var totalPrice = optimizeResult.totalPrice;
            return View(optimizeResult);
            // Return a view or appropriate response with the results
        }

        // GET method for BuyLivestock
        public IActionResult BuyLivestock(int idCoop)
        {
            var viewModel = new BuyLivestockViewModel
            {
                IdCoop = idCoop
            };

            return View(viewModel);
        }

        // POST method for BuyLivestock
        [HttpPost]
        public IActionResult BuyLivestock(BuyLivestockViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Use the viewModel values for the purchase
                var name = viewModel.Name;
                var genderInput = viewModel.GenderInput;
                var input = viewModel.Input;
                var price = viewModel.Price;
                var totalPrice = 0.0;

                var buyLivestockResult = _apiGoatClient.BuyLivestock(name, genderInput, input, price, viewModel.IdCoop, ref totalPrice);
                var buyLivestockMessage = buyLivestockResult;

                return RedirectToAction("BuyLivestockResult", new { message = buyLivestockMessage });
                // Redirect to a result view with the appropriate message
            }

            // If the model is not valid, return the view with validation errors
            return View(viewModel);
        }
    }
}
