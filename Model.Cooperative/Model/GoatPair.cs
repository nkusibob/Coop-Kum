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
        // FK properties (REQUIRED)
        public int? FirstGoatLivestockId { get; set; }
        public int? SecondGoatLivestockId { get; set; }
        public Goat FirstGoat { get; set; }
        public Goat SecondGoat { get; set; }
    }

}
