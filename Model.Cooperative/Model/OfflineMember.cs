using System.Collections.Generic;

namespace Model.Cooperative
{
    public class OfflineMember : BasicMember
    {
        public OfflineMember()
        {
            
        }
        public virtual ICollection<PersonPicture> Images { get; set; }
        public virtual Person Person { get; set; }
        public virtual Coop MyCoop { get; set; }
    }
}