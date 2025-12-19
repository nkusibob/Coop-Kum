using Model.Cooperative.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Cooperative
{
    public class StepProject
    {
        public StepProject()
        {
            NbreOfDays = 0; // Set a default value for NbreOfDays
        }
        
        public int StepProjectId { get; set; }
        public bool VetVisit { get; set; }
        public bool? Reviewed { get; set; }
        public int NbreOfDays { get; set; }

        [ForeignKey(nameof(StepCategorieId))]
        public int StepCategorieId { get; set; } // Foreign key property
        public StepCategorie StepCategorie { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;


        public decimal StepBudget { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public int SelectedStepCategoryId { get; set; }
        public DateTime StartingDate { get; set; } 
        public DateTime ReviewDate
        {
            get
            {
                return StartingDate.AddDays(NbreOfDays);
            }
        }
        public int? EmployeeId { get; set; }     // nullable  step can exist before assignment
    

        public virtual Employee Employee { get; set; }

    }
}