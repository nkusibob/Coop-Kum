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
        protected abstract int GetGestationPeriod();


        public (List<(Livestock, Livestock)> pairs, List<Goat> pregnantFemales, string message) CreateLivestockPairs(List<Livestock> livestocks)
        {
            List<(Livestock, Livestock)> pairs = new List<(Livestock, Livestock)>();
            List<Goat> pregnantFemales = livestocks
                .Where(p => p is Goat && p.IsPregnant)
                .Cast<Goat>()
                .ToList();
            string message = "";

           

            List<Livestock> females = livestocks
                .Where(l => l.Gender == LivestockGender.Female && l.Age >= MinimumFemaleAgeInYears && !l.IsPregnant)
                .ToList();

            List<Livestock> males = livestocks
                .Where(l => l.Gender == LivestockGender.Male && l.Age >= MinimumMaleAgeInYears)
                .ToList();

            if (females.Count == 0 || males.Count == 0)
            {
                message = "There are no compatible livestock for breeding.";
                return (pairs, pregnantFemales, message);
            }

            int maleIndex = 0; // Index to track the current male being paired

            while (females.Count > 0)
            {
                var male = males[maleIndex];

                var potentialMates = females
                    .Where(female => male.CanMarry(female, 4) && male.NumFemalesPaired < GetMaxFemalesPerMale())
                    .ToList();

                if (potentialMates.Count > 0)
                {
                    // Select a random female from the potential mates
                    int randomIndex = new Random().Next(potentialMates.Count);
                    var mate = potentialMates[randomIndex];

                    var pair = (mate, male);
                    pairs.Add(pair);

                    if (mate.LastDropped.GetValueOrDefault().AddYears(1) <= DateTime.UtcNow)
                    {
                        mate.IsPregnant = true;
                        mate.LastDropped = DateTime.UtcNow.AddDays(GetGestationPeriod());

                        male.NumFemalesPaired++;

                        pregnantFemales.Add((Goat)mate);
                    }

                    // Remove the selected female from the females list
                    females.Remove(mate);
                }

                // Move to the next male for pairing
                maleIndex = (maleIndex + 1) % males.Count;
            }

            if (pairs.Count == 0)
            {
                message = "No breeding pairs were found.";
            }

            return (pairs, pregnantFemales, message);
        }

        protected bool HasCommonAncestor(Livestock livestock1, Livestock livestock2, int maxGenerations)
        {
            // Implement common ancestor logic here
            return false;
        }
    }
}
