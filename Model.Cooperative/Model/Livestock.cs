using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Model.Cooperative
{
    public enum LivestockType
    {
        Goat,
        Cow,
        Sheep,
        Horse
        // Add more types as needed
    }
    public enum LivestockGender
    {
        Male,
        Female,
        Unknown
    }

    [Table("Livestock")]
    public abstract class Livestock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LivestockId { get; set; }

        // Constructor
        protected Livestock(string Name)
        {
            Kids = new List<Livestock>();
            // Initialize the Kids collection
        }


        [Required]
        public string LivestockType { get; set; }

        [Required]
        public string Name { get; set; }

        public double Age { get; set; }

        [NotMapped]
        public DateTime Birthday => DateTime.Today.AddYears(-(int)Age);

        public bool IsSold { get; set; }

        public double Price { get; set; }

        public bool IsAlive { get; set; } = true;

        public bool IsPregnant { get; set; }

        public int? MotherId { get; set; }

        public int? FatherId { get; set; }

        public DateTime? LastDropped { get; set; }

        public LivestockGender Gender { get; set; }

        [ForeignKey("MotherId")]
        public virtual Livestock Mother { get; set; }

        [ForeignKey("FatherId")]
        public virtual Livestock Father { get; set; }

        [InverseProperty("Father")]
        public virtual ICollection<Livestock> Kids { get; set; }

        public int NumFemalesPaired { get; set; }

        public virtual Coop Cooperative { get; set; }

        [ForeignKey("CoopId")]
        public int CoopId { get; set; }

        public virtual List<Livestock> GetKids()
        {
            var kidsWithParents = new List<Livestock>();
            foreach (var kid in Kids)
            {
                var kidWithParents = Activator.CreateInstance(this.GetType(), kid.Name, kid.Gender, kid.Age, kid.LastDropped, null, this, kid.Father) as Livestock;
                kidsWithParents.Add(kidWithParents);
            }
            return kidsWithParents;
        }

        public virtual bool CanMarry(Livestock livestock)
        {
            return false;
        }

        public void Sell(double sellPrice)
        {
            IsSold = true;
            Price = sellPrice;
        }
    }
}