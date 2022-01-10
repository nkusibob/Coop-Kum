using Business.Cooperative.BusinessModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative
{
    public class ProjectionPerPeriod
    {
        public ProjectionPerPeriod()
        {
            Projects = new HashSet<Project>();

        }
        public decimal NbreOfMonth { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
