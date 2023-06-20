using System;
using System.Collections.Generic;
using Business.Cooperative.Interfaces;

namespace Business.Cooperative.BusinessModel
{
    public class BusinessCoopManager : BusinessPerson, IManager
    {
        public BusinessCoopManager()
        {
            Project = new BusinessProject();
        }

        public virtual BusinessProject Project { get; set; }
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