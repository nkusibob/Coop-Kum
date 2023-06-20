using System;
using Business.Cooperative.Interfaces;

namespace Business.Cooperative.BusinessModel
{
    public class Employee : BusinessPerson, IEmployee
    {
        public Employee()
        {
            Step = new StepProject();
        }

        public decimal Salary { get; set; }

        public StepProject Step { get; set; }

        public virtual decimal CalculatePerStepSalary(int rank)
        {
            return Convert.ToDecimal((rank * 1.50));
        }
    }
}