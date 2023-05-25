using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Cooperative
{
    public class Employee
    {

        [Key]
        public int EmployeeId { get; set; }


        public decimal DailySalary { get; set; }

        public Employee()
        {
            Person = new Person();
        }

        public decimal CurrentStepEmployeeSalary
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

        // Modified properties
        public virtual Person Person { get; set; }
        public   StepProject Step { get; set; }
        public virtual ICollection<StepProject> Steps { get; set; }

    }


}