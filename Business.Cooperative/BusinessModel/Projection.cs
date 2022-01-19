using System.Collections.Generic;

namespace Business.Cooperative
{
    public class Projection
    {
        public Projection()
        {
            ProjectionsPerYear = new HashSet<Projections>();
        }

        public decimal GlobalProjectedBenefit { get; set; }

        public virtual ICollection<Projections> ProjectionsPerYear { get; set; }
    }
}