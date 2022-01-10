using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Cooperative.BusinessModel
{
    public class Employee :Person, IEmployee
    {
        public Employee()
        {
            Step = new StepProject();
        }
        public decimal Salary { get; set; }
        
        public  StepProject Step { get; set; }

        public virtual decimal CalculatePerStepSalary(int rank)
        {
            return Convert.ToDecimal((rank * 1.50));
            
        }

    }
}
