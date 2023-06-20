using System;
using System.Collections.Generic;

namespace Business.Cooperative.BusinessModel
{
    public class BusinessCoop
    {
        public string Name { get; set; }

        public BusinessCoop()
        {
            Projects = new HashSet<BusinessProject>();
            Membres = new HashSet<BusinessMembre>();
        }

        public virtual ICollection<BusinessProject> Projects { get; set; }
        public virtual ICollection<BusinessMembre> Membres { get; set; }
        public virtual decimal Budget { get; set; }

       
    }
}