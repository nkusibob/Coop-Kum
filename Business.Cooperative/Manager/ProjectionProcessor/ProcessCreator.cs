using System.Collections.Generic;

namespace Business.Cooperative
{
    public static class ProcessCreator
    {
        public static List<Projection> GetProjectionList()
        {
            return new List<Projection>();
        }

        public static GoalProcessor GetGoalProcessor(List<Projection> projections)
        {
            return new GoalProcessor(projections);
        }

        public static ProjectionPerDuration GetProjectionPerDuration()
        {
            return new ProjectionPerDuration();
        }

        public static ProjectionPerPeriod GetProjectionPerPeriod(Goal goal, decimal res)
        {
            _ = goal.GoalToReach < 0 ? res-- : res;
            var ppp = new ProjectionPerPeriod()
            {
                NbreOfMonth = res,
                Projects = goal.Projects
            };
            return ppp;
        }
    }
}