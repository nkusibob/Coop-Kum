using Business.Cooperative.BusinessModel;
using System;
using System.Collections.Generic;
using System.Text;

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
