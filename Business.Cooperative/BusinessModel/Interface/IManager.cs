using System.Collections.Generic;

namespace Business.Cooperative.BusinessModel
{
    public interface IManager : IEmployee
    {
        //project steps and cost report
        List<Employee> Employees { get; set; }

        void GetProductionPlanReview(List<Employee> employees);

        decimal ProjectBudget { get; set; }
        decimal ExpenseBudget { get; set; }
        Project Project { get; set; }
        public decimal AfterStepBudget { get; set; }
    }
}