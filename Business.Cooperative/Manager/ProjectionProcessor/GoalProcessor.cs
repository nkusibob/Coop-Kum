using Business.Cooperative.ProjectState;
using System.Collections.Generic;

namespace Business.Cooperative
{
    public class GoalProcessor
    {
        private List<Projections> projections;

        public GoalProcessor(List<Projections> projections)
        {
            this.projections = projections;
        }

        public List<Projections> GetProjectionByGoal(Goal goal)
        {
            var context = new Context(new ProjectSetOn(goal));
            projections = context.Request();
            return projections;
        }
    }
}