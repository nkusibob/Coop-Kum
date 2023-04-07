using Business.Cooperative.ProjectState;
using System.Collections.Generic;

namespace Business.Cooperative
{
    public class GoalProcessor
    {
        private List<Projection> projections;

        public GoalProcessor(List<Projection> projections)
        {
            this.projections = projections;
        }

        public List<Projection> GetProjectionByGoal(Goal goal)
        {
            var context = new Context(new ProjectSetOn(goal));
            projections = context.Request();
            return projections;
        }
    }
}