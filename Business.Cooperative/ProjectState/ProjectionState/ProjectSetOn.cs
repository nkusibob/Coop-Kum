using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Cooperative.ProjectState
{
    public class ProjectSetOn : State
    {
        private readonly Goal goal;

        public ProjectSetOn(Goal goal)
        {
            this.goal = goal;
        }
        public override List<Projections> Handle(Context context)
        {
            int count = 0;
            List<Projections> output = new List<Projections>();
            var projectList = goal.Projects;
            while ((goal.GoalToReach >= 0) && (projectList.Count -1 >= count))
            {
                ProjectionPerDuration prjdur = new ProjectionPerDuration(goal,output);
                output = prjdur.GetNbreMonthPerProject(projectList, 1);
                decimal res = output.Sum(x => x.GeneratedProduction);
                goal.GoalToReach -= res;
            }
            return output;
        }

    }
}
