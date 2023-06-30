using Model.Cooperative;
using Model.Cooperative.Model;
using System.Collections.Generic;

namespace Model.Cooperative
{
    public class BreedViewModel
    {
        public List<GoatPair> EligiblePairs { get; set; }
        public List<Goat> PregnantGoats { get; set; }
        public string Message { get; set; }
    }


}
