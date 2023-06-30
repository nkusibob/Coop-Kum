using Model.Cooperative;
using System.Collections.Generic;

namespace Model.Cooperative
{
    public class HandleBirthViewModel
    {
        public List<Goat> AllNewBorn { get; set; }
        public List<Goat> PregnantGoats { get; set; }
        public string Message { get; set; }
    }

}
