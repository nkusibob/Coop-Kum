using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Cooperative.BusinessModel
{
    public class Coop
    {
        public string Name { get; set; }
        public Coop()
        {
            Projects = new HashSet<Project>();
            Membres = new HashSet<Membre>();
        }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Membre> Membres { get; set; }
        public virtual decimal Budget { get; set; }


    }
}
