using Business.Cooperative.Interfaces;
using Model.Cooperative;
using Model.Cooperative.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

       
        public override async Task<Goat> RemoveLivestockByName(string livestockName,int idCoop)
        {
            goats = await goatRepository.GetGoatAsync(idCoop);
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
            return goat;
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
                //goat.IsAlive = false;
                //goats.Remove(goat);
                goatRepository.RemoveGoat(goat);

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

        public async Task<List<Goat>> GetGoatsAsync(int coopId)
        {
            if (goats == null || !goats.Any())
            {
                goats = await goatRepository.GetGoatAsync(coopId);
            }

            return goats;
            
        }


        public async Task<(List<(Goat, Goat)> eligiblePairs, List<Goat> pregnantGoats, string message)> MateGoatsAsync(int idCoop)
        {
            var goatFactory = new GoatFactory();
            var pairsTuple = goatFactory.CreateGoatPairs(await GetGoatsAsync(idCoop));
            var pairs = pairsTuple.Item1;
            var pregnantGoats = pairsTuple.Item2.OfType<Goat>().ToList();
            var message = pairsTuple.Item3;

            List<(Goat, Goat)> eligiblePairs = new List<(Goat, Goat)>();

            foreach (var pair in pairs)
            {
                var mother = pair.Item1;
                var father = pair.Item2;
                goatRepository.UpdateGoat((Goat)mother);
                goatRepository.UpdateGoat((Goat)father);

                if (mother.LastDropped.GetValueOrDefault().Date == DateTime.Now.Date)
                {
                    eligiblePairs.Add(((Goat)mother, (Goat)father));
                }
            }

            return (eligiblePairs, pregnantGoats, message);
        }

        public List<Goat> HandleBirth( Goat mother, Goat father, List<string> kidNames, List<LivestockGender> kidGenders)
        {
            
            var newBorn = mother.GiveBirth(mother,father,kidNames,kidGenders).ToList();
            int kidCount = Math.Min(newBorn.Count, 3); // Limit the kid count to a maximum of 3
            foreach (var kid in newBorn)
            {
                goatRepository.AddGoat(kid);
                goatRepository.SaveChanges();
            }
            
            foreach (var kid in newBorn)
            {
                NotifyGoatBirth(mother.Name, kid.Name, kid.Gender.ToString(), DateTime.Now.Date);
            }
            return newBorn;
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
