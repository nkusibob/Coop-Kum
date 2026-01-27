using System.Collections.Generic;
using System.Linq;

namespace Business.Cooperative
{
    public static class SimulationProcessor
    {
        public static ProjectProduction GetSimulationForPeriod(Goal goal)
        {
            List<Projection> projectionsList = ProcessCreator.GetProjectionList();
            GoalProcessor glpr = ProcessCreator.GetGoalProcessor(projectionsList);
            projectionsList = glpr.GetProjectionByGoal(goal);

            var firstProjection = projectionsList.FirstOrDefault();
            if (firstProjection == null)
            {
              
                throw new InvalidOperationException("No projection matches the given goal.");

              
            }

            decimal res = firstProjection.numberOfMonth;

            ProjectionPerDuration prj = ProcessCreator.GetProjectionPerDuration();
            ProjectionPerPeriod prd = ProcessCreator.GetProjectionPerPeriod(goal, res);

            return prj.GetProjectionPerPeriod(prd);
        }
    }
}