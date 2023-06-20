using Business.Cooperative.Interfaces;
using Model.Cooperative;
using Model.Cooperative.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Business.Cooperative
{
    public class GoatHerd :Herd
    {
        private List<Goat> goats;
        private HashSet<IGoatObserver> observers;
        private List<Goat> allKids = new List<Goat>();
        private readonly GoatRepository goatRepository;


        public GoatHerd(GoatRepository goatRepository)
        {
            
            observers = new HashSet<IGoatObserver>();
            this.goatRepository = goatRepository;
            goats = new List<Goat>();
        }

        public void AddGoat(Livestock livestock)
        {
            goats.Add((Goat)livestock);
            goatRepository.AddGoat((Goat)livestock);
            goatRepository.SaveChanges();

        }

        public override void AddLivestock(Livestock livestock)
        {
            AddGoat(livestock);
        }

       
        public override void RemoveLivestockByName(string livestockName)
        {
            var goat = goats.FirstOrDefault(n => n.Name == livestockName);
            if (goat == null)
            {
                throw new ArgumentException($"There is no goat with the name {livestockName} in the goatHerd.");
            }
            else
            {
                goat.IsAlive = false;
                goats.Remove(goat);
                goatRepository.RemoveGoat(goat);
                goatRepository.SaveChanges();
                NotifyGoatDeath(DateTime.Now.AddYears(-(int)goat.Age), goat.Name, goat.Gender.ToString(), DateTime.Now);
            }
        }

        public override void RemoveLivestockToSellByName(string livestockName, double price)
        {
            var goat = goats.FirstOrDefault(n => n.Name == livestockName);
            if (goat == null)
            {
                throw new ArgumentException($"There is no goat with the name {livestockName} in the goatHerd.");
            }
            else
            {
                goat.IsAlive = false;
                goats.Remove(goat);

                goatRepository.SaveChanges();
                NotifyGoatSold(DateTime.Now.AddYears(-(int)goat.Age), goat.Name, goat.Gender.ToString(), price);
            }
        }

        public void NotifyGoatDeath(DateTime deadGoatBirthDate, string deadGoatName, string deadGoatGender, DateTime deathDate)
        {
            foreach (var observer in observers)
            {
                observer.NotifyLivestockDeath(deadGoatBirthDate, deadGoatName, deadGoatGender, deathDate);
            }
        }

        public void NotifyGoatSold(DateTime soldGoatBirthDate, string soldGoatName, string soldGoatGender, double price)
        {
            foreach (var observer in observers)
            {
                observer.NotifyLivestockSold(soldGoatBirthDate, soldGoatName, soldGoatGender, price);
            }
        }

        public List<Goat> GetGoats(int coopId)
        {
            return base.GetLivestock(goatRepository.GetGoat(coopId).ToList());

             
        }

        public List<Goat> MateGoats(int kidCount, LivestockGender kidGender, string kidName, int idCoop)
        {
            var goatFactory = new GoatFactory();
            var pairs = goatFactory.CreateGoatPairs(GetGoats(idCoop));
            List<Goat> kids = new List<Goat>();

            foreach (var pair in pairs)
            {
                var mother = pair.Item1;
                var father = pair.Item2;
                var birthDate = DateTime.Now;

                for (int i = 0; i < kidCount; i++)
                {
                    kids.Add(new Goat(kidName, kidGender, 0, null,LivestockType.Goat, new List<Goat>(), mother, father));
                }


                mother.GiveBirth(birthDate, kids, father);

                foreach (var kid in kids)
                {
                    NotifyGoatBirth(mother.Name, kid.Name, kid.Gender.ToString(), birthDate);
                }
            }

            goats.AddRange(kids);

            return kids;
        }

        public void NotifyGoatBirth(string motherName, string kidName, string kidGender, DateTime birthDate)
        {
            foreach (var observer in observers)
            {
                observer.NotifyLivestockBirth(motherName, kidName, kidGender, birthDate);
            }
        }
        public override int GetLivestockCount()
        {
            return livestocks.Count(l => l is Goat);
        }

        public override int GetFemaleCount()
        {
            return livestocks.Count(l => l is Goat && l.Gender == LivestockGender.Female);
        }

        public override int GetMaleCount()
        {
            return livestocks.Count(l => l is Goat && l.Gender == LivestockGender.Male);
        }

        public override List<Livestock> GetMaleLivestocks()
        {
            List<Livestock> maleLivestocks = new List<Livestock>();
            foreach (Livestock livestock in livestocks)
            {
                if (livestock is Goat goat && goat.Gender == LivestockGender.Male)
                {
                    maleLivestocks.Add(goat);
                }
            }
            return maleLivestocks;
        }

        public override Livestock GetLivestockByName(string name)
        {
            foreach (var livestock in livestocks)
            {
                if (livestock.Name == name)
                {
                    return livestock;
                }
            }

            return null; // Livestock not found
        }

        
        public override void AddObserver(IGoatObserver observer)
        {
            observers.Add(observer);
        }

    }
}
