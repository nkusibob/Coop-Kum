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


        protected Goat(string name) :base (name)
        {
            // this constructor is required by entity framework core
            // and will be used for entity creation.
        }

        public Goat(,string name, LivestockGender gender, double age, DateTime? lastDropped, LivestockType type, List<Goat> goats, Livestock mother = null, Livestock father = null)
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

        public void GiveBirth(DateTime birthDate, IEnumerable<Goat> kids, Goat father)
        {
            if (!IsAlive)
            {
                throw new InvalidOperationException($"{Name} is dead and cannot give birth.");
            }

            if (!IsPregnant)
            {
                throw new InvalidOperationException($"{Name} is not pregnant.");
            }

            if (birthDate < LastDropped.GetValueOrDefault().Add(new TimeSpan(150, 0, 0, 0)))
            {
                throw new InvalidOperationException($"{Name} cannot give birth yet.");
            }

            if (Age > 7)
            {
                throw new InvalidOperationException($"{Name} is too old to give birth.");
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

                kid.Mother = this;
                kid.Father = father;
                // Handle birth process
            }

            IsPregnant = false;
                kid.Mother.LastDropped = DateTime.UtcNow.AddMonths(GetGestationPeriod());
            LastDropped = birthDate;

            if (birthDate.AddYears(1) <= DateTime.UtcNow)
            {
                IsPregnant = true;
                LastDropped = birthDate;
                // Handle pregnancy continuation case
            }
        }
    }
}
