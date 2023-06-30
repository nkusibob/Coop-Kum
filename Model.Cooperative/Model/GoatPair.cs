using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative.Model
{
    public class GoatPair
    {
        [Key]
        public int GoatPairId { get; set; }
        public Goat FirstGoat { get; set; }
        public Goat SecondGoat { get; set; }
    }

}
