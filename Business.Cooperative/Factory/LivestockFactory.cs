using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Cooperative
{
    public abstract class LivestockFactory
    {
        protected abstract int MinimumFemaleAgeInYears { get; }
        protected abstract double MinimumMaleAgeInYears { get; }
        protected abstract int MinimumMonthsSinceLastBirth { get; }
        protected abstract int GetMaxFemalesPerMale();

        public  List<(Livestock, Livestock)> CreateLivestockPairs(List<Livestock> livestocks)
        {
            List<(Livestock, Livestock)> pairs = new List<(Livestock, Livestock)>();

            List<Livestock> females = livestocks
                .Where(l => l.Gender == LivestockGender.Female && l.Age >= MinimumFemaleAgeInYears && !l.IsPregnant)
                .ToList();

            List<Livestock> males = livestocks
                .Where(l => l.Gender == LivestockGender.Male && l.Age >= MinimumMaleAgeInYears)
                .ToList();

            if (females.Count == 0 || males.Count == 0)
            {
                throw new Exception("There are no compatible livestock for breeding.");
            }

            foreach (var female in females)
            {
                if (female.LastDropped.GetValueOrDefault().AddMonths(MinimumMonthsSinceLastBirth) > DateTime.UtcNow)
                {
                    var expectedNextBreedingDate = female.LastDropped.GetValueOrDefault().AddYears(1);
                    throw new Exception($"{female.Name} cannot breed yet. Expected date of next breeding: {expectedNextBreedingDate}");
                }

                var potentialMates = males
                    .Where(male => female.CanMarry(male) && male.NumFemalesPaired < GetMaxFemalesPerMale())
                    .ToList();

                if (potentialMates.Count() > 0)
                {
                    int randomIndex = new Random().Next(potentialMates.Count());
                    var mate = potentialMates[randomIndex];

                    pairs.Add((female, mate));
                    female.IsPregnant = true;
                    mate.NumFemalesPaired++;

                    return pairs;
                }
            }

            throw new Exception("No breeding pairs were found.");
        }

        protected bool HasCommonAncestor(Livestock livestock1, Livestock livestock2, int maxGenerations)
        {
            // Implement common ancestor logic here
            return false;
        }
    }
}
