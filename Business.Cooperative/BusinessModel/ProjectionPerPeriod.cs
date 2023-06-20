using Business.Cooperative.BusinessModel;
using System.Collections.Generic;

namespace Business.Cooperative
{
    public class ProjectionPerPeriod
    {
        public ProjectionPerPeriod()
        {
            Projects = new HashSet<BusinessProject>();
        }

        public decimal NbreOfMonth { get; set; }
        public virtual ICollection<BusinessProject> Projects { get; set; }
    }
}