using System.Collections.Generic;
using Web.Cooperation.Models.ViewModel;

namespace Model.Cooperative
{
    public class PeopleCoop
    {
        public decimal SumFees { get; set; }
        public decimal TotalExpected { get; set; }

        public decimal TotalExpenses { get; set; }
        public List<Membre> PersonList { get; set; }
        public List<ProjectBoard> ProjectBoardList { get; set; }
        public List<OfflineMember> OfflineMembers { get; set; }
        public List<Livestock> Livestocks { get; set; }
    }
    public class ExpenseByCategoryAndDateModel
    {
        public string Category { get; set; }
        public string Month { get; set; }
        public decimal Total { get; set; }
    }

}