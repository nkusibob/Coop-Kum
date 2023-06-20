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

        protected override int MinimumFemaleAgeInYears => 1;
        protected override double MinimumMaleAgeInYears => 0.67;
        protected override int MinimumMonthsSinceLastBirth => 9;

        protected override int GetMaxFemalesPerMale()
        {
            return MAX_FEMALES_PER_MALE;
        }
        public List<(Goat, Goat)> CreateGoatPairs(List<Goat> goats)
        {
            var livestock = goats.Cast<Livestock>().ToList();
            var pairs = CreateLivestockPairs(livestock);
            var goatPairs = pairs.Select(pair => (pair.Item1 as Goat, pair.Item2 as Goat)).ToList();
            return goatPairs;
        }


    }
}

