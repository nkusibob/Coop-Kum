using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string FirstName { get; set; }
        public int IdNumber { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FullName
        { get { return String.Format("Firstname is: {0}, Surname is {1}", FirstName, LastName); } }
        public string PersonImageUrl { get; set; }

    }
}