using Business.Cooperative.BusinessModel;
using System.Collections.Generic;
using System.Linq;

namespace Business.Cooperative
{
    public class ProjectionPerDuration
    {
        private readonly Goal goal;
        private readonly List<Projection> projectionsList = new List<Projection>();

        public ProjectionPerDuration(Goal goal, List<Projection> projectionsList)
        {
            this.goal = goal;
            this.projectionsList = projectionsList;
        }

        public ProjectionPerDuration()
        {
        }

        public ProjectProduction GetProjectionPerPeriod(ProjectionPerPeriod prPeriod)
        
        {
            List<Projection> projectionsList = GetProjectedPeriodList(prPeriod.Projects, prPeriod.NbreOfMonth);
            return new ProjectProduction()
            {
                globalProjectedBenefit = projectionsList.Sum(x => x.generatedProduction),
                projectionsPerYear = projectionsList
            };
        }

        public List<Projection> GetProjectedPeriodList(ICollection<Project> projects, decimal duration)
        {
            foreach (Project prj in projects)
            {
                decimal benefitPerDuration = prj.ProjectBudget * prj.Efficiency;
                decimal benifitPerYear = (benefitPerDuration / prj.DurationInMonth) * duration;
                projectionsList.Add(new Projection()
                {
                    generatedProduction = benifitPerYear,
                    projectName = prj.Name,
                    numberOfMonth = duration
                });
            }
            return projectionsList;
        }

        public List<Projection> GetNbreMonthPerProject(ICollection<Project> projects, decimal duration)
        {
            var prjList = (ICollection<Projection>)GetProjectedPeriodList(projects, duration);
            return prjList.GroupBy(p => p.generatedProduction, p => p.numberOfMonth,
                                   (key, g) => new Projection
                                   {
                                       generatedProduction = key,
                                       numberOfMonth = g.Sum(),
                                   }).ToList();
        }
    }
}