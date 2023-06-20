using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Model.Cooperative
{
    public class GoatRepository
    {
        private readonly CooperativeContext context;

        public GoatRepository(CooperativeContext context)
        {
            this.context = context;
        }
        public IQueryable<Goat> GetGoat(int coopId)
        {
            IQueryable<Goat> goatsWithNames = context.Livestock
                .Where(p => p.CoopId == coopId && p.LivestockType == "Goat")
                .Select(p => new Goat(p.Name, p.Gender, p.Age, p.LastDropped, LivestockType.Goat, null, null, null)
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
                });

            return goatsWithNames;
        }
        public void AddGoat(Goat goat)
        {
            context.Goat.Add(goat);
        }

        public void RemoveGoat(Goat goat)
        {
            context.Goat.Remove(goat);
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
