using System.Collections.Generic;
using Web.Cooperation.Models.ViewModel;

namespace Model.Cooperative
{
    public class PeopleCoop
    {
        public decimal SumFees { get; set; }
        public List<Membre> PersonList { get; set; }
        public List<ProjectBoard> ProjectBoardList { get; set; }
        public List<OfflineMember> OfflineMembers { get; set; }
    }
}