using System.Collections.Generic;
using Web.Cooperation.Models.ViewModel;

namespace Model.Cooperative
{
    public class ConnectedPerson
    {
        public List<Person> PersonList { get; set; }
        public List<ProjectBoard> ProjectBoardList { get; set; }
        public List<OfflineMember> OfflineMembers { get; set; }
    }
}