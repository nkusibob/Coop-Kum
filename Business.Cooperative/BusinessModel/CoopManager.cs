using System;
using System.Collections.Generic;

namespace Business.Cooperative.BusinessModel
{
    public class CoopManager : Person, IManager
    {
        public CoopManager()
        {
            Project = new Project();
        }

        public virtual Project Project { get; set; }
        public List<Employee> Employees { get; set; }
        public decimal ProjectBudget { get; set; }
        public decimal Salary { get; set; }
        public decimal ExpenseBudget { get; set; }
        public decimal AfterStepBudget { get; set; }
        public decimal CalculatePerStepSalary(int rank)
        {
            decimal baseAmount = 1;
            return baseAmount + Convert.ToDecimal((rank * 1.50));
        }
        public void GetProductionPlanReview(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                DateTime endDate = startDate.AddDays((double)employee.Step.NbreOfDays);
                employee.Step.ReviewDate = String.Format("{0:ddd, MMM d, yyyy}", endDate);
            }
        }
    }
}