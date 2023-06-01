using System;


namespace Model.Cooperative
{
    public class StepProject
    {
        public StepProject()
        {
            NbreOfDays = 0; // Set a default value for NbreOfDays
        }
        
        public int StepProjectId { get; set; }

        public int NbreOfDays { get; set; }

        public virtual Project project { get; set; }
        public decimal StepBudget { get; set; }
        public string Description { get; set; }
        public DateTime StartingDate { get; set; } 
        public DateTime ReviewDate
        {
            get
            {
                return StartingDate.AddDays(NbreOfDays);
            }
        }
    
        public virtual Employee Employee { get; set; }

    }
}