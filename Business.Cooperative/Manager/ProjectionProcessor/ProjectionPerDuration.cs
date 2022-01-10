using Business.Cooperative.BusinessModel;
using System.Collections.Generic;
using System.Linq;

namespace Business.Cooperative
{
    public class ProjectionPerDuration 
    {
        private readonly Goal goal;
        private readonly List<Projections> projectionsList = new List<Projections>();
        public ProjectionPerDuration(Goal goal, List<Projections> projectionsList)
        {
            this.goal = goal;
            this.projectionsList = projectionsList;
        }
        public ProjectionPerDuration()
        {

        }
        public Projection GetProjectionPerPeriod(ProjectionPerPeriod prPeriod)
        {
            List<Projections> projectionsList = GetProjectedPeriodList(prPeriod.Projects, prPeriod.NbreOfMonth);
            return new Projection()
            {
                GlobalProjectedBenefit = projectionsList.Sum(x => x.GeneratedProduction),
                ProjectionsPerYear = projectionsList
            };

        }
        public List<Projections> GetProjectedPeriodList(ICollection<Project> projects, decimal duration)
        {
            foreach (Project prj in projects)
            {
                decimal benefitPerDuration = prj.ProjectBudget * prj.Efficiency;
                decimal benifitPerYear = (benefitPerDuration / prj.DurationInMonth) * duration;
                projectionsList.Add(new Projections()
                {
                    GeneratedProduction = benifitPerYear,
                    ProjectName = prj.Name,
                    NumberofMonth = duration

                });
            }
            return projectionsList;
        }
        public List<Projections> GetNbreMonthPerProject(ICollection<Project> projects, decimal duration)
        {
            var  prjList = (ICollection<Projections>) GetProjectedPeriodList(projects, duration);
            return prjList.GroupBy(p => p.GeneratedProduction,p => p.NumberofMonth,
                                   (key, g) => new Projections
                                      {
                                        GeneratedProduction = key,
                                        NumberofMonth = g.Sum(),
                                      }).ToList();
        }
        
    }
}
