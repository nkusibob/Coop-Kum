using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Cooperative.BusinessModel
{
    public class Membre :Person, IMembre
    {
        public Membre()
        {
            MyCoop = new Coop();
        }
        public virtual Coop MyCoop { get; set; }

        public decimal MemberShipFees { get ; set ; }
        

    }
}
