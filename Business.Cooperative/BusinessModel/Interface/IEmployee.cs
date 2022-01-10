using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.BusinessModel
{
    public interface IEmployee 
    {
        decimal Salary { get; set; }
        decimal CalculatePerStepSalary(int rank);
    }
}
