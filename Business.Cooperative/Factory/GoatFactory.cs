using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Cooperative
{
    public class GoatFactory : LivestockFactory
    {
        public const int MAX_FEMALES_PER_MALE = 50;
        public const int GoatGestationDays = 150;


        protected override int MinimumFemaleAgeInYears => 1;
        protected override double MinimumMaleAgeInYears => 0.67;
        protected override int MinimumMonthsSinceLastBirth => 9;

        protected override int GetMaxFemalesPerMale()
        {
            return MAX_FEMALES_PER_MALE;
        }
        public (List<(Livestock, Livestock)> pairs, List<Goat> pregnantFemales, string message) CreateGoatPairs(List<Goat> goats)
        {
            var livestockList = goats.Cast<Livestock>().ToList();
            var pairTuple = CreateLivestockPairs(livestockList);
            return pairTuple;
        }

        protected override int GetGestationPeriod()
        {
            return GoatGestationDays;
        }
    }
}

