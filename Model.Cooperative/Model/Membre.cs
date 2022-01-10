using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative
{
    public class Membre 
    {
        public Membre()
        {
            Person = new Person();
            MyCoop = new Coop();

        }

        [Key]
        public int MembreId { get; set; }
        public decimal FeesPerYear { get; set; } = 10;
        public virtual Person Person { get; set; }
        public virtual Coop MyCoop { get; set; }

    }
}
