using Business.Cooperative.BusinessModel;
using System.Collections.Generic;

namespace Business.Cooperative
{
    public class Goal
    {
        public Goal()
        {
            Projects = new HashSet<Project>();
        }

        public decimal GoalToReach { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}