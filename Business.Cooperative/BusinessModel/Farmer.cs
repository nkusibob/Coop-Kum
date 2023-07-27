using Business.Cooperative.Interfaces;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cooperative
{


    public class Farmer : IGoatObserver
    {
        private GoatHerd goatHerd;
        //private ILogger<Farmer> logger;

        public Farmer(GoatHerd herd/*, ILogger<Farmer> logger*/)
        {
            goatHerd = herd;
            //this.logger = logger;

            herd.AddObserver(this);
        }

        public string NotifyLivestockBirth(string motherName, string kidName, string kidGender, DateTime birthDate)
        {
            return $"Congratulations! {motherName} gave birth to a {kidGender} named {kidName} on {birthDate:d}.";
        }

        public string NotifyLivestockDeath(DateTime deadGoatbirtDate, string deadGoatName, string deadGoatGender, DateTime deathDate)
        {
            string pronoun = deadGoatGender.ToLower() == "male" ? "he" : "she";
            return $"Sad news! A {deadGoatGender.ToLower()} goat named {deadGoatName} with a birthdate of {deadGoatbirtDate:d} has passed away on {deathDate:d}. Time to eat it, remembering {deadGoatGender.ToLower()} for its contributions to the goatHerd.";
        }

        public string BuyGoat(Goat goat)
        {
            goatHerd.AddGoat(goat);
            return $"Bought {goat.Name} for the goatHerd.";
        }

        public async Task MateGoatsAsync(int kidCount, LivestockGender kidGender, string kidName, int idCoop)
        {
            await goatHerd.MateGoatsAsync(idCoop);
        }

        public string NotifyLivestockSold(DateTime soldGoatBirthDate, string soldGoatName, string soldGoatGender, decimal price)
        {
            string genderSuffix = soldGoatGender == "Male" ? "he" : "she";
            string priceString = price.ToString("C2", CultureInfo.CurrentCulture);
            return $"{soldGoatName} has been sold for {priceString}! {genderSuffix} was born on {soldGoatBirthDate.ToString("d")}";
        }
    }

}
