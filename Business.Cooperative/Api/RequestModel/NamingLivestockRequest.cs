using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.Api.RequestModel
{
    public class NamingLivestockRequest
    {
        public List<LivestockGender> KidGenders { get; set; }
        public List<string> KidNames { get; set; }
    }

}
