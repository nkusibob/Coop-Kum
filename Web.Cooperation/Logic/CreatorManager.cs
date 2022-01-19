using Model.Cooperative;
using System.Collections.Generic;
using Web.Cooperation.Models.ViewModel;

namespace Web.Cooperation.Logic
{
    public static class CreatorManager
    {
        public static GetCoopBoard CreateCoopBoard(CooperativeContext context)
        {
            return new GetCoopBoard(context);
        }
        public static List<ProjectBoard> CreateProjectBoardList()
        {
            return new List<ProjectBoard>();

        }
        public static ProjectBoard CreateProjectBoard()
        {
            return new ProjectBoard();

        }
    }
}