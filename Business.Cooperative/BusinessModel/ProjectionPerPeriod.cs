using Business.Cooperative.BusinessModel;
using System.Collections.Generic;

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