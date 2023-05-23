using Business.Cooperative;
using Model.Cooperative;
using System.Collections.Generic;
using Web.Cooperation.Logic;
using Web.Cooperation.Models.ViewModel;

namespace Web.Cooperation.Helper
{
    public static class ComputingProjectionHelper
    {

        public  static void GetProjectionForCurrentProject(Project project, out ProjectBoard projectBoard, out ProjectionPerPeriod projection, GetCoopBoard getCoopBoard)
        {
            projectBoard = getCoopBoard.GetProjectBoard(project);
            projection = new ProjectionPerPeriod
            {
                NbreOfMonth = projectBoard.Project.DurationInMonth,
                Projects = new List<Business.Cooperative.BusinessModel.Project>
                {
                    new Business.Cooperative.BusinessModel.Project
                    {
                        Name = projectBoard.Project.Name,
                        Efficiency = projectBoard.Project.Efficiency,
                        DurationInMonth = projectBoard.Project.DurationInMonth,
                        ProjectBudget = projectBoard.Project.ProjectBudget
                    }
                }
            };
        }
    }
}
