using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Cooperative
{
//    [SwaggerSchema("Goat")] attribute on the Goat class.
//    This attribute is related to Swagger, which is a tool
//    for documenting and generating API documentation.

//The[SwaggerSchema("Goat")] attribute is used to specify
//the schema name for the Goat class in the Swagger documentation.
//It helps to provide more specific information about the class
//when generating API documentation.

//By adding this attribute, you are instructing Swagger
//to use the name "Goat" for the schema of the Goat class
//in the generated API documentation.This can be helpful
//for better organization and clarity in the documentation.

//Note that the[SwaggerSchema] attribute is specific
//to Swagger and may not have any direct impact on the
//functionality or behavior of your code. It is primarily
//used for documentation purposes.

    [SwaggerSchema("Goat")]
    public class Goat : Livestock
    {
        private LivestockGender gender;
        private DateTime now;
        private Goat[] goats;

        // Default constructor
        protected Goat() : base(string.Empty)
        {
            // This constructor is required for deserialization
            // You can initialize any properties if needed
        }
        protected Goat(string name) :base (name)
        {
            // this constructor is required by entity framework core
            // and will be used for entity creation.
        }

        public Goat(string name, LivestockGender gender, double age, DateTime? lastDropped, string type, List<Goat> goats, Livestock mother = null, Livestock father = null)
        : base(name)
        {
            // Initialize other properties
            Gender = gender;
            Age = age;
            LastDropped = lastDropped;
            LivestockType = "Goat";
            Mother = mother as Goat;
            Father = father as Goat;
        }

        public Goat(string name, LivestockGender gender, LivestockType type, int age,int coopId,  DateTime now, Goat[] goats) : this(name)
        {
            this.gender = gender;
            Age = age;
            this.now = now;
            this.goats = goats;
            LivestockType = "Goat";
            CoopId = coopId;
        }

        public IEnumerable<Goat> GiveBirth(Goat mother, Goat father, List<string> kidNames, List<LivestockGender> kidGenders)
        {
            if (!IsAlive)
            {
                throw new InvalidOperationException($"{Name} is dead and cannot give birth.");
            }

            if (!IsPregnant)
            {
                throw new InvalidOperationException($"{Name} is not pregnant.");
            }

            if (Age > 7)
            {
                throw new InvalidOperationException($"{Name} is too old to give birth.");
            }

            int kidCount = Math.Min(kidNames.Count, 3); // Limit the kid count to a maximum of 3

            List<Goat> kids = new List<Goat>();

            for (int i = 0; i < kidCount; i++)
            {
                var kidName = kidNames[i];
                var kidGender = kidGenders[i];
                //"goat", 0, null, new List<Goat>(), mother, father
                var kid = new Goat(kidName, kidGender, 0, null, "Goat", new List<Goat>(), mother, father);

                //var kid = new Goat(kidName, kidGender,"goat",0,mother.Cooperative.IdCoop,DateTime.Today,null);
                kids.Add(kid);
            }

            foreach (var kid in kids)
            {
                if (kid.Age < 0)
                {
                    throw new ArgumentException($"Age of {kid.Name} cannot be negative.");
                }

                if (kid.Mother != null && !kid.Mother.IsAlive)
                {
                    throw new InvalidOperationException($"{kid.Name} cannot have a dead mother.");
                }

                kid.Mother = mother;
                kid.Father = father;
                // Handle birth process
            }

            IsPregnant = false;
            LastDropped = DateTime.Now;

            if (LastDropped.Value.AddYears(1) <= DateTime.UtcNow)
            {
                IsPregnant = true;
                LastDropped = DateTime.Now;
                // Handle pregnancy continuation case
            }

            return kids;
        }
    }
}
