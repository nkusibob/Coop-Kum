
using Business.Cooperative.Interfaces;
using Microsoft.Extensions.Logging;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Cooperative
{
    public abstract class Herd
    {
        protected List<Livestock> livestocks;
        private readonly List<Livestock> kids;

        public Herd()
        {
            livestocks = new List<Livestock>();
            kids = new List<Livestock>();
        }

        public abstract void AddLivestock(Livestock livestock);
        public abstract void RemoveLivestockByName(string livestockName);
        public abstract void RemoveLivestockToSellByName(string livestockName, double price);
        public abstract void AddObserver(IGoatObserver observer);

        public List<T> GetLivestock<T>(List<T> livestock) where T : Livestock
        {
            List<T> result = new List<T>();
            List<T> mothers = new List<T>();
            List<T> mothersKids = new List<T>();
            List<string> output = new List<string>();

            // Identify mothers and their kids
            foreach (var animal in livestock)
            {
                if (animal.GetKids().Count > 0)
                {
                    foreach (var kid in animal.GetKids())
                    {
                        if (kid.Mother == animal)
                        {
                            if (!mothers.Contains(animal))
                            {
                                mothers.Add(animal);
                            }
                            break;
                        }
                    }
                }
            }

            // Display mothers and their kids
            foreach (var mother in mothers)
            {
                string motherInfo = $"- {mother.Name} ({mother.Gender}, {mother.Age} years old)";
                output.Add(motherInfo);

                if (mother.Age == 0)
                {
                    DateTime birthDate = mother.Birthday;
                    TimeSpan age = DateTime.Today - birthDate;
                    int days = age.Days;
                    int months = (int)Math.Round(age.TotalDays / 30);
                    string ageInfo = days < 30 ? $"  Age: {days} days old" : $"  Age: {months} months old";
                    output.Add(ageInfo);
                }

                output.Add("    Kids:");
                foreach (var kid in mother.GetKids())
                {
                    if (kid.Mother == mother)
                    {
                        string kidInfo = $"      - {kid.Name} ({kid.Gender} ";
                        if (kid.Father != null)
                        {
                            kidInfo += $"(Father: {kid.Father.Name})";
                        }
                        output.Add(kidInfo);

                        if (kid.Age == 0)
                        {
                            DateTime birthDate = kid.Birthday;
                            TimeSpan age = DateTime.Today - birthDate;
                            int days = age.Days;
                            int months = (int)Math.Round(age.TotalDays / 30);
                            string ageInfo = days < 30 ? $"        Age: {days} days old" : $"        Age: {months} months old";
                            output.Add(ageInfo);
                            mothersKids.Add((T)kid);
                        }
                    }
                }

                result.Add(mother);
            }

            // Display remaining animals (excluding kids)
            foreach (var animal in livestock)
            {
                if (!mothersKids.Any(m => m.Name == animal.Name ||
                                          (m.Mother != null && animal.Mother != null && m.Mother.Name == animal.Mother.Name && m.Age == animal.Age))
                    && !result.Contains(animal))
                {
                    string animalInfo = $"- {animal.Name} ({animal.Gender}, {animal.Age} years old)";
                    output.Add(animalInfo);
                    result.Add(animal);
                }
            }

            // Print the output to the console or perform any other desired action
            foreach (var line in output)
            {
                //logger.LogInformation(line);
                //Console.WriteLine(line);
            }

            return result;
        }

        // Other members of the class

        public abstract int GetLivestockCount();

        public abstract int GetFemaleCount();

        public abstract int GetMaleCount();
        public abstract List<Livestock> GetMaleLivestocks();

        public abstract Livestock GetLivestockByName(string name);
    }

}
