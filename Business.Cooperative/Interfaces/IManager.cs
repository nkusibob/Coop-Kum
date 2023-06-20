using System.Collections.Generic;
using Business.Cooperative.BusinessModel;

namespace Business.Cooperative.Interfaces
{
    public interface IManager : IEmployee
    {
        //project steps and cost report
        List<Employee> Employees { get; set; }

        void GetProductionPlanReview(List<Employee> employees);

        decimal ProjectBudget { get; set; }
        decimal ExpenseBudget { get; set; }
        BusinessProject Project { get; set; }
        public decimal AfterStepBudget { get; set; }
    }
}