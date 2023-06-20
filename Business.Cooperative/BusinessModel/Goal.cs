using Business.Cooperative.BusinessModel;
using System.Collections.Generic;

namespace Business.Cooperative
{
    public class Goal
    {
        public Goal()
        {
            Projects = new HashSet<BusinessProject>();
        }

        public decimal GoalToReach { get; set; }
        public virtual ICollection<BusinessProject> Projects { get; set; }
    }
}