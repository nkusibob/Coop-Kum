using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cooperative
{
    public class GoatRepository
    {
        private readonly CooperativeContext context;

        public GoatRepository(CooperativeContext context)
        {
            this.context = context;
        }
        public async Task<List<Goat>> GetGoatAsync(int coopId)
        {
            List<Goat> goatsWithNames = await context.Livestock
                .Where(p => p.CoopId == coopId && p.LivestockType == "Goat")
                .Select(p => new Goat(p.Name, p.Gender, p.Age, p.LastDropped, "Goat", null, null, null)
                {
                    Name = p.Name,
                    IsSold = p.IsSold,
                    Price = p.Price,
                    IsAlive = p.IsAlive,
                    IsPregnant = p.IsPregnant,
                    MotherId = p.MotherId,
                    FatherId = p.FatherId,
                    NumFemalesPaired = p.NumFemalesPaired,
                    CoopId = p.CoopId
                })
                .ToListAsync();

            return goatsWithNames;
        }

        



        public void AddGoat(Goat goat)
        {
            context.Goat.Add(goat);
            SaveChanges();
        }
        public void UpdateGoat(Goat updatedGoat)
        {
            var existingGoat = context.Goat.Where(x=> x.Name==updatedGoat.Name).FirstOrDefault(); // Assuming 'Id' is the primary key property

            if (existingGoat == null)
            {
                // Handle the case when the existing goat is not found
                throw new ArgumentException("Goat not found");
            }

            // Update the properties of the existing goat with the values from the updated goat
            existingGoat.NumFemalesPaired = updatedGoat.NumFemalesPaired;
            existingGoat.IsPregnant = updatedGoat.IsPregnant;
            existingGoat.LastDropped = updatedGoat.LastDropped;

      
            // Update other properties as needed

            SaveChanges();
        }

        public void RemoveGoat(Goat goat)
        {
            var soldGoat = context.Goat.FirstOrDefault(x => x.Name == goat.Name);

            if (soldGoat != null)
            {
                context.Goat.Remove(soldGoat);
                SaveChanges();
            }
        }

        // Add other repository methods as needed

        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (SqlException sql)
            {
                

                throw sql; ;
            }
        }
    }

}
