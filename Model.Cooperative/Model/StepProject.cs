using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Cooperative
{
    public class StepProject
    {
        [Key]
        public int StepProjectId { get; set; }

        public int NbreOfDays { get; set; }
        public decimal StepBuget { get; set; }
        public string Description { get; set; }
        public DateTime StartingDate { get; set; } 
        public DateTime ReviewDate
        {
            get
            {
                return StartingDate.AddDays(NbreOfDays);
            }
        }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

    }
}