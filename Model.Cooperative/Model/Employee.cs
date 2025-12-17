using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Cooperative
{
    public class Employee
    {
        private Person person;

        [Key]
        public int EmployeeId { get; set; }


        public decimal DailySalary { get; set; }

        public Employee()
        {
     
        }

        public Employee(Person person, decimal dailySalary)
        {
            this.person = person;
            DailySalary = dailySalary;
        }

        public decimal CurrentEmployeeAllStepsSalary
        {
            get
            {
                decimal totalDays = 0;
                if (Steps != null && Steps.Count > 0)
                {
                    foreach (var step in Steps)
                    {
                        totalDays += step.NbreOfDays;
                    }
                }
                return DailySalary * totalDays;
            }
        }

        public virtual CoopManager Manager { get; set; }

        // New properties
       
        public int SelectedPersonId { get; set; }
        // Property to hold the selected StepCategorie ID
        public int? StepProjectId { get; set; }
        public int PersonId { get; set; }
        // Modified properties
        public virtual Person Person { get; set; }
        public   StepProject Step { get; set; }
        public virtual ICollection<StepProject> Steps { get; set; }

    }


}