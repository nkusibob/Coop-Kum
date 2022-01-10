using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.BusinessModel
{
    public abstract class  Person
    {
         public string FirstName { get; set; }
         public int IdNumber { get; set; }
         public string LastName { get; set; }
    }
}
