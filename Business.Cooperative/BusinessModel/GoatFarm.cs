using Business.Cooperative.Api;
using Microsoft.AspNetCore.Identity;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace Business.Cooperative.BusinessModel
{
    public class GoatFarm : IFarm<Goat>
    {
        private GoatHerd _goatHerd;
        private List<Goat> _goatList;
        public GoatFarm(GoatHerd goatHerd)

        {
            _goatHerd = goatHerd;
            // Retrieve the goats once during construction

        }

        public List<Goat> ListAvailableLivestock(int CoopId)
        {
            // Logic to list available goats
            _goatList = _goatHerd.GetGoats(CoopId);
            
            // Return the list of goats
            return _goatList.ToList();
        }


        public void BreedLivestock(int kidCount, LivestockGender kidGender, string kidName, int IdCoop)
        {
            _goatHerd.MateGoats(kidCount,kidGender,kidName,IdCoop);
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

        public void EatLivestock(string eatenGoatName)
        {
            _goatHerd.RemoveLivestockByName(eatenGoatName);
        }

       
         public string OptimizeHerdGrowth(bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice,string goatName)
            {
                var goatCount = _goatHerd.GetLivestockCount();
                var femaleCount = _goatHerd.GetFemaleCount();
                var maleCount = _goatHerd.GetMaleCount();

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
                        string nameSelected = goatName;
                        Goat goat = GetValidMaleGoat(nameSelected);
                        goatsToKeep.Add(goat);
                    }
                }
            int malesToSell = maleCount - malesToKeep;

            // Sell goats if required
            if (sellGoats)
            {
                double ageInMonths = 8;
                var goatsToSell = _goatHerd.GetMaleLivestocks()
                    .Where(g => g.Age <= ageInMonths / 12 * 8 && !goatsToKeep.Contains(g))
                    .OrderBy(g => g.Age)
                    .Take(malesToSell);

                foreach (var goat in goatsToSell)
                {
                    goat.Sell(sellPrice);
                    _goatHerd.RemoveLivestockToSellByName(goat.Name, sellPrice);
                }

                double totalPrice = goatsToSell.Where(g => g.IsSold).Sum(g => g.Price);
            }


            // Rest of the code remains the same

            // Return the result as a string or throw an exception
            return "Herd optimization completed successfully.";
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
