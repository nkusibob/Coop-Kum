using System.Collections.Generic;
using System.Linq;

namespace Business.Cooperative.ProjectState
{
    public class ProjectSetOn : State
    {
        private readonly Goal goal;

        public ProjectSetOn(Goal goal)
        {
            this.goal = goal;
        }

        public override List<Projection> Handle(Context context)
        {
            int count = 0;
            List<Projection> output = new List<Projection>();
            var projectList = goal.Projects;
            while ((goal.GoalToReach >= 0) && (projectList.Count - 1 >= count))
            {
                ProjectionPerDuration prjdur = new ProjectionPerDuration(goal, output);
                output = prjdur.GetNbreMonthPerProject(projectList, 1);
                decimal res = output.Sum(x => x.generatedProduction);
                goal.GoalToReach -= res;
            }
            return output;
        }
    }
}