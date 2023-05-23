using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public class Coop
    {
        [Key]
        public int IdCoop { get; set; }

        public Coop()
        {
            Projects = new HashSet<Project>();
            Membres = new HashSet<Membre>();
            OfflineMembers = new HashSet<OfflineMember>();
        }

        public string CoopName { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Membre> Membres { get; set; }
        public virtual ICollection<OfflineMember> OfflineMembers { get; set; }
        public virtual decimal Budget { get; set; }
    }
}