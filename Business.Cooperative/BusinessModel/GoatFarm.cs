using Business.Cooperative.Api;
using Microsoft.AspNetCore.Identity;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Business.Cooperative.BusinessModel
{
    public class GoatFarm : IFarm<Goat>
    {
        private GoatHerd _goatHerd;
        private List<Goat> _goatList;
        public const int GoatGestationPeriod = 50;
        private (List<(Goat, Goat)> eligiblePairs, List<Goat> pregnantGoats, string message) tupleEligibleCouples;
        public GoatFarm(GoatHerd goatHerd)

        {
            _goatHerd = goatHerd;
            // Retrieve the goats once during construction

        }

        public async Task<List<Goat>> ListAvailableLivestock(int CoopId)
        {
            // Check if _goatList is already populated
            if (_goatList == null)
            {
                // Logic to list available goats
                _goatList =await  _goatHerd.GetGoatsAsync(CoopId);
            }

            // Return the list of goats
            return _goatList;
        }


        public async Task<(List<(Goat, Goat)> eligiblePairs, List<Goat> pregnantGoats, string message)> BreedLivestockAsync(int IdCoop)
        {

            // Check if _goatList is already populated
            if (tupleEligibleCouples.eligiblePairs == null)
            {
                // Logic to list available goats
                tupleEligibleCouples = await _goatHerd.MateGoatsAsync(IdCoop);
            }

            // Return the list of goats
            return tupleEligibleCouples;
        }
        public async Task<(List<Goat> allNewBorn, List<Goat> pregnantGoats, string message)> HandleBirth(int IdCoop, List<LivestockGender> kidGenders, List<string> kidNames)
        {
            List<Goat> allNewBorn = new List<Goat>();
            List<Goat> pregnantGoats = new List<Goat>();
            string message = "";

            if (tupleEligibleCouples.eligiblePairs == null)
            {
                // Logic to list available goats
                tupleEligibleCouples = await BreedLivestockAsync(IdCoop);
            }

            foreach (var couple in tupleEligibleCouples.eligiblePairs)
            {
                var coupleNewBorn = _goatHerd.HandleBirth(couple.Item1, couple.Item2, kidNames, kidGenders);
                allNewBorn.AddRange(coupleNewBorn);
            }

            pregnantGoats = tupleEligibleCouples.pregnantGoats;
            message = tupleEligibleCouples.message;

            return (allNewBorn, pregnantGoats, message);
        }
        public string BuyLivestock(string name, string genderInput, string input, double price,int idCoop, ref double totalPrice)
        {
           
                if (!Enum.TryParse<LivestockGender>(genderInput, out var gender))
                {
                    throw new ArgumentException("Invalid gender input.");
                }

                DateTime birthdate;
                int age;
                if (DateTime.TryParse(input, out birthdate))
                {
                    age = DateTime.Now.Year - birthdate.Year;
                    if (DateTime.Now < birthdate.AddYears(age))
                    {
                        age--;
                    }
                }
                else if (double.TryParse(input, out var doubleAge))
                {
                    age = (int)doubleAge;
                }
                else
                {
                    throw new ArgumentException("Invalid input. Enter a valid birthdate of the goat (yyyy-MM-dd) or its age as double.");
                }

                double totalPriceToPay = price;
                if (totalPrice > 0 && price > totalPrice)
                {
                    totalPriceToPay = 0;
                    totalPrice -= price;
                }
                else if (totalPrice > 0 && price <= totalPrice)
                {
                    totalPriceToPay = price - totalPrice;
                    totalPrice = 0;
                }

                var goat = new Goat(name, gender, LivestockType.Goat, age,idCoop, DateTime.Now, new Goat[] { });
                _goatHerd.AddGoat(goat);

                string result = $"Added {name} ({gender}, {age} years old) to the herd for {price}.";

                if (totalPriceToPay > 0)
                {
                    result += $" You still owe {totalPriceToPay}.";
                }

                return result;
            

        }

        public async Task<Goat> EatLivestock(string eatenGoatName, int idCoop)
        {
           var eatenGoat= await  _goatHerd.RemoveLivestockByName(eatenGoatName,idCoop);
           return eatenGoat;
        }


        public async Task<(List<Goat> goatsToKeep, double totalPrice)> OptimizeHerdGrowthAsync(bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice, string goatName, int idCoop)
        {
            var goatList = await ListAvailableLivestock(idCoop);
            var goatCount = goatList.Count();
            var femaleCount = goatList.Count(l => l.Gender == LivestockGender.Female);
            var maleCount = _goatList.Count(l => l.Gender == LivestockGender.Male);

            int optimalMaleCount = (int)Math.Ceiling((double)femaleCount / 50);

            if (extendGenetics)
            {
                if (malesToKeep < 0 || malesToKeep > maleCount)
                {
                    throw new ArgumentException($"Invalid input. Please enter a number between 0 and {maleCount}.");
                }
            }

            List<Goat> goatsToKeep = new List<Goat>();

            if (extendGenetics)
            {
                for (int i = 0; i < malesToKeep; i++)
                {
                    Goat goat = _goatList.FirstOrDefault(l => l.Gender == LivestockGender.Male && l.Name == goatName);
                    goatsToKeep.Add(goat);
                }
            }

            int malesToSell = maleCount - malesToKeep;

            // Sell goats if required
            double totalPrice = 0;
            IEnumerable<Goat> goatsToSell = new List<Goat> ();
            if (sellGoats)
            {
                double ageInMonths = 8;
                 goatsToSell = goatList
                    .Where(g => g.Age <= ageInMonths / 12 * 8 && !goatsToKeep.Contains(g))
                    .OrderBy(g => g.Age)
                    .Take(malesToSell);

                foreach (var goat in goatsToSell)
                {
                    goat.Price = sellPrice;
                    goat.Sell();
                    _goatHerd.RemoveLivestockToSellByName(goat.Name, sellPrice);
                    totalPrice += goat.Price;
                }
            }

            
            return (goatsToSell.ToList(), totalPrice);
        }
        private string GetGoatNameInput(string prompt, string goatName)
          {
            if (string.IsNullOrWhiteSpace(goatName))
            {
              throw new InvalidOperationException("Goat name input is required.");
            }

                return goatName;
         }
         private Goat GetValidMaleGoat(string nameSelected)
         {
            Goat goat = (Goat)_goatHerd.GetLivestockByName(nameSelected);
            if (goat != null && goat.Gender == LivestockGender.Male)
            {
                return goat;
            }
            throw new ArgumentException("Invalid input. Please enter a valid male goat name.");
         }

    }
}
