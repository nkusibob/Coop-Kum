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

        [RegularExpression("^[0-9]*$", ErrorMessage = "IdentificationNumber must contain only numbers.")]
        public string  IdentificationNumber { get; set; }

        // Constructor
        protected Livestock(string Name)
        {
            Kids = new List<Livestock>();
            // Initialize the Kids collection
        }

        public virtual ICollection<Image> Images { get; set; }

        [Required]
        public string LivestockType { get; set; }

        [Required]
        public string Name { get; set; }

        public double Age { get; set; }

        public DateTime? PurchaseDate { get; set; }

        [NotMapped]
        public DateTime Birthday => DateTime.Today.AddYears(-(int)Age);

        private bool isSold;

        public bool IsSold
        {
            get { return isSold; }
            set
            {
                if (isSold != value)
                {
                    isSold = value;
                    if (isSold)
                    {
                        SellDate = DateTime.Now;
                    }
                }
            }
        }


        public decimal Price { get; set; }

        public bool IsAlive { get; set; } = true;

        public bool IsPregnant { get; set; }

        public int? MotherId { get; set; }

        public int? FatherId { get; set; }

        public DateTime? LastDropped { get; set; }

        public DateTime? SellDate { get; set; }


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
        public string Color { get; set; }

        public decimal Weight { get; set; }
       
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

        public virtual bool CanMarry(Livestock livestock, int maxGenerationalDifference)
        {
            // Check if the provided livestock is a sibling
            if (livestock.MotherId != null && livestock.FatherId != null &&
                livestock.MotherId == MotherId && livestock.FatherId == FatherId)
            {
                // The provided livestock is a sibling, so they cannot marry
                return false;
            }

            // Check if the provided livestock is a direct ancestor within the allowed generational difference
            var currentLivestock = this;
            for (int generation = 0; generation <= maxGenerationalDifference; generation++)
            {
                if ((livestock.MotherId != null && currentLivestock.MotherId == livestock.LivestockId) ||
                    (livestock.FatherId != null && currentLivestock.FatherId == livestock.LivestockId))
                {
                    // The provided livestock is a direct ancestor within the allowed generational difference,
                    // so they cannot marry
                    return false;
                }

                if (currentLivestock.Mother != null)
                {
                    currentLivestock = currentLivestock.Mother;
                }
                else
                {
                    // Reached the end of the ancestry chain
                    break;
                }
            }

            // The provided livestock is not a sibling or a direct ancestor within the allowed generational difference,
            // so they can potentially marry
            return true;
        }


        public void Sell()
        {
            IsSold = true;
            SellDate = DateTime.Now;
        }
    }
   

}