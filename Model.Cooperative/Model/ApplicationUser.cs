using Microsoft.AspNetCore.Identity;

namespace Model.Cooperative
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public int IdNumber { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}