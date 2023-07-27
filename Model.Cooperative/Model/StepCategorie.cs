using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Cooperative.Model
{
    public class StepCategorie
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty(nameof(StepProject.StepCategorie))]
        public ICollection<StepProject> StepProjects { get; set; } // Collection of StepProject objects

        public StepCategorie(string name)
        {
            Name = name;
        }
    }
}
