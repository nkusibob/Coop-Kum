using Model.Cooperative;
using System;
using System.Collections.Generic;

namespace Web.Cooperation.Logic
{
    public class GoatService
    {
        private const decimal SPACE_PER_GOAT_AND_LAMB = 1.20m;  // Space required per goat and lamb in square meters
        private const decimal SPACE_PER_ADULT_GOAT = 0.80m;  // Space required per adult goat in square meters
        private const decimal SPACE_PER_RAM = 2.0m;  // Space required per ram in square meters
        private const decimal FOOD_QUANTITY_GOAT_AND_YOUNG = 0.3m;  // Food quantity for goat and its young in kilograms
        private const decimal FOOD_QUANTITY_WEANED_YOUNG_GOAT = 0.1m;  // Food quantity for weaned young goat in kilograms
        private const decimal FOOD_QUANTITY_GOATS_IN_COVERING_AGE = 0.3m;  // Food quantity for goats in covering age in kilograms
        private const decimal MEAT_PRODUCTION_FACTOR = 0.48m;  // Meat production factor (48% of weight)
        private const decimal MANURE_PRODUCTION_PER_GOAT = 2000m;  // Manure production per year for three goats in kilograms

        public decimal CalculateFoodQuantity(List<Livestock> goats)
        {
            decimal totalFoodQuantity = 0;

            foreach (var goat in goats)
            {
                if (goat.Age <= 1)
                    totalFoodQuantity += FOOD_QUANTITY_GOAT_AND_YOUNG;
                else if (goat.Age <= 3)
                    totalFoodQuantity += FOOD_QUANTITY_WEANED_YOUNG_GOAT;
                else if (goat.Age > 3 && goat.Age <= 6)
                    totalFoodQuantity += FOOD_QUANTITY_GOATS_IN_COVERING_AGE;
            }

            return totalFoodQuantity; // in kilograms
        }

        public decimal CalculateShelterSize(List<Livestock> goats)
        {
            decimal totalSpaceRequired = 0;

            foreach (var goat in goats)
            {
                decimal spaceRequired = SPACE_PER_GOAT_AND_LAMB;

                if (goat.Age >= 1 && goat.Age <= 3)
                    spaceRequired += SPACE_PER_ADULT_GOAT;

                if (goat.Age > 3 && goat.Gender == LivestockGender.Female)
                    spaceRequired += SPACE_PER_RAM;

                totalSpaceRequired += spaceRequired; // in square meters
            }

            return totalSpaceRequired;
        }

        public decimal CalculateMeatProduction(decimal weight)
        {
            return weight * MEAT_PRODUCTION_FACTOR; // in kilograms
        }

        public decimal CalculateManureProduction(int numberOfGoats)
        {
            return (numberOfGoats / 3) * MANURE_PRODUCTION_PER_GOAT; // in kilograms
        }
        public int CalculateProjectedGoatCount(int initialFemaleCount, int initialMaleCount, int numberOfYears, decimal mortalityPercentage, decimal averageKidsPerBirth)
        {
            // Convert the mortality percentage to a survival rate (percentage of kids that survive).
            decimal survivalRate = 1 - (mortalityPercentage / 100);

            // Initialize variables to keep track of the female and male goat counts.
            decimal femaleCount = initialFemaleCount;
            decimal maleCount = initialMaleCount;

            // Variable to track the total number of kids born.
            decimal totalKids = 0;

            // Loop through each year to calculate the projected goat count.
            for (int i = 1; i <= numberOfYears; i++)
            {
                // Calculate the total number of kids born in this year based on the total number of adult goats,
                // average kids per birth, and the survival rate of kids.
                decimal totalBirths = (femaleCount + maleCount) * averageKidsPerBirth * survivalRate;

                // Add the number of kids born in this year to the total kids count.
                totalKids += totalBirths;

                // Update the female and male goat counts based on the new kids born in this year.
                femaleCount += totalBirths;
                maleCount += totalBirths;
            }

            // Calculate the projected goat count at the end of the given number of years
            // by summing the initial female count, initial male count, and total kids born.
            return (int)Math.Round(initialFemaleCount + initialMaleCount + totalKids);
        }

    }
}
