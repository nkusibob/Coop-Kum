using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public class StepProject
    {
        [Key]
        public int StepProjectId { get; set; }

        public decimal NbreOfDays { get; set; }
        public decimal StepBuget { get; set; }
        public string Description { get; set; }
        public String ReviewDate { get; set; }
    }
}