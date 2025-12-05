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
                    IdentificationNumber = p.IdentificationNumber,
                    NumFemalesPaired = p.NumFemalesPaired,
                    CoopId = p.CoopId
                })
                .ToListAsync();

            return goatsWithNames;
        }

        



        public async Task AddGoat(Goat goat)
        {
            context.Goat.Add(goat);
            await context.SaveChangesAsync();
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
            existingGoat.IdentificationNumber = updatedGoat.IdentificationNumber;

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

        public async Task<Livestock> GetGoat(int livestockId)
        {
            var foundGoat = await context.Goat.Include(x => x.Images).FirstOrDefaultAsync(x => x.LivestockId == livestockId);
            return foundGoat;
        }

        public async Task<Goat> UpdateGoat(int livestockId, Goat goat)
        {
            var existingGoat = await context.Goat.FindAsync(livestockId);

            if (existingGoat == null)
            {
                // Livestock with the given ID was not found
                return null;
            }

            if (existingGoat.LastDropped != goat.LastDropped && goat.LastDropped != null)
            {
                existingGoat.LastDropped = goat.LastDropped;
            }

            // Remove existing LivestockPicture entities not present in the updated goat
            var updatedPictures = goat.Images.Select(img => new LivestockImage
            {
                LivestockId = livestockId,
                Data = img.Data
                // Set other properties of the LivestockPicture entity as needed
            }).ToList();


            // Add or update the updated pictures
            foreach (var updatedPicture in updatedPictures)
            {


                var existingPicture = context.LivestockImages
        .FirstOrDefault(p => p.LivestockId == livestockId && p.Data == updatedPicture.Data);

                if (existingPicture == null)
                {
                    context.LivestockImages.Add(updatedPicture);
                }

            }

            // Update the properties of the existing goat with the new values
            if (goat.Images.Count()>0)
            {
                existingGoat.Images.ToList().AddRange(goat.Images);

            }           
            existingGoat.IdentificationNumber = goat.IdentificationNumber;
            existingGoat.Name = goat.Name;
            existingGoat.Age = goat.Age;
            existingGoat.IsSold = goat.IsSold;
            existingGoat.Price = goat.Price;
            existingGoat.Weight = goat.Weight;
            existingGoat.Color = goat.Color;

            // Update other properties as needed

            // Save the changes to the database
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return existingGoat;
        }

    }

}
