using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative
{
    public class ProjectProduction
    {
        public decimal globalProjectedBenefit { get; set; }
        public List<Projection> projectionsPerYear { get; set; }
    }
}
