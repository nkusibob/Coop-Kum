using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.Api.RequestModel
{
    public class GoatLivestock
    {
        public int LivestockId { get; set; }
        public string LivestockType { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsSold { get; set; }
        public decimal Price { get; set; }
        public bool IsPregnant { get; set; }
        public DateTime LastDropped { get; set; }
        public int NumFemalesPaired { get; set; }
        public int CoopId { get; set; }
        public List<byte[]> Images { get; set; }        
        // Add additional properties as needed

        // Constructor
        public GoatLivestock()
        {
            Images = new List<byte[]>();
        }
    }

}
