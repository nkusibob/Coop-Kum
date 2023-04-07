using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        public decimal Salary { get; set; }

        public Employee()
        {
            Step = new StepProject();
            Person = new Person();
        }
        public decimal StepSalary
        {
            get
            {
                return Salary * Step.NbreOfDays;
            }
        }
        public virtual Project Project { get; set; }
        public CoopManager Manager { get; set; }

        public virtual Person Person { get; set; }
        public virtual StepProject Step { get; set; }
    }
}