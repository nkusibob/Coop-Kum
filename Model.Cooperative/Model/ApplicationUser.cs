using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Model.Cooperative.Model;
using System.Collections.Generic;

namespace Model.Cooperative
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string IdNumber { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int  Fees { get; set; }
        public virtual ApplicationUserImage UserImage { get; set; }


        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}