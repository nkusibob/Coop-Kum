using Business.Cooperative.Api;
using Business.Cooperative.Api.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        public async Task<IActionResult> OptimizeHerdGrowth(int idCoop, bool extendGenetics, int malesToKeep, bool sellGoats, decimal sellPrice, string goatName)
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
                decimal totalPrice =0 ;
                var buyLivestockResult = _apiGoatClient.BuyLivestock(name, genderInput, input, price, viewModel.IdCoop, totalPrice);
                var buyLivestockMessage = buyLivestockResult;

                return RedirectToAction("Details", "Coops");
                // Redirect to a result view with the appropriate message
            }

            // If the model is not valid, return the view with validation errors
            return View(viewModel);
        }

        // GET: Livestock/UpdateDetails/5
        public async Task<IActionResult> UpdateDetails(int livestockId)
        {
            

            var detailedGoat = await _apiGoatClient.UpdateDetails(livestockId);
            if (detailedGoat == null)
            {
                return NotFound();
            }

            return View(detailedGoat);
        }

        // POST: Livestock/UpdateDetails/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDetails(int livestockId, [Bind("LivestockId, LivestockType, Name, Age, IsSold, Price, IsPregnant, LastDropped, NumFemalesPaired,IdentificationNumber,Weight, Color, CoopId")] Goat goat, List<IFormFile> imageFiles)
        {
            if (livestockId != goat.LivestockId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Convert the uploaded images to byte arrays
                    List<byte[]> imageDatas = new List<byte[]>();

                    foreach (var imageFile in imageFiles)
                    {
                        byte[] imageData = null;
                        if (imageFile != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await imageFile.CopyToAsync(memoryStream);
                                imageData = memoryStream.ToArray();
                            }
                        }
                        imageDatas.Add(imageData);
                    }

                    // Create a list of LivestockImage objects from the byte arrays
                    List<LivestockImage> livestockImages = new List<LivestockImage>();

                    foreach (var imageData in imageDatas)
                    {
                        LivestockImage livestockImage = new LivestockImage
                        {
                            Data = imageData,
                            LivestockId = livestockId
                        };
                        livestockImages.Add(livestockImage);
                    }

                    // Assign the image list to the goat
                    goat.Images = livestockImages;

                    // Pass the goat object to the API client for update
                    var updatedGoat = await _apiGoatClient.UpdateDetails(livestockId, goat);

                    // Pass the updated goat object to the view or redirect to appropriate action
                    return RedirectToAction("Details", "Coops");
                }
                catch (Exception ex)
                {
                    // Handle any API exception or display error messages
                    ModelState.AddModelError("", "An error occurred while updating the goat details: " + ex.Message);
                }
            }

            // If the model is not valid, return the view with validation errors
            return View(goat);
        }





    }
}
