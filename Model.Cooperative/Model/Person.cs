using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative
{
    public  class Person
    {
       
        [Key]
        public int PersonId { get; set; }
        public string FirstName { get ; set ; }
        public int IdNumber { get ; set ; }
        public string LastName { get ; set ; }
    }
}
