using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Cooperative
{
    public static class SimulationProcessor
    {
        public static Projection GetSimulationForPeriod(Goal goal)
        {
            List<Projections> projectionsList = ProcessCreator.GetProjectionList();
            GoalProcessor glpr = ProcessCreator.GetGoalProcessor(projectionsList);
            projectionsList = glpr.GetProjectionByGoal(goal);
            decimal res = projectionsList.FirstOrDefault().NumberofMonth;
            ProjectionPerDuration prj = ProcessCreator.GetProjectionPerDuration();
            ProjectionPerPeriod prd =ProcessCreator.GetProjectionPerPeriod(goal,res);
            return prj.GetProjectionPerPeriod(prd);
        }

       
    }
}
