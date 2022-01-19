using Business.Cooperative.BusinessModel;
using System.Collections.Generic;

namespace Business.Cooperative.ProjectState.ProjectStep
{
    public class ProjectStarted : StepState
    {
        private IManager manager;

        public ProjectStarted(IManager manager)
        {
            this.manager = manager;
        }

        public override IManager Handle(StepContext stepContext)
        {
            List<Employee> managedEmpList = new List<Employee>();
            managedEmpList.AddRange(manager.Employees);
            manager.AfterStepBudget = manager.Project.ProjectBudget - manager.ExpenseBudget;
            manager.GetProductionPlanReview(managedEmpList);
            return manager;
            //foreach (Employee employee in employees)
            //{
            //    StepProject sp = new StepProject();
            //    sp.Description = "Initialisation";
            //    sp.StepBuget = 50;
            //    employee.Step = sp;
            //    employee.AssignManager(manager);

            //    employee.Salary = employee.CalculatePerDayRate(3) * .DurationInMonth;
            //    managedEmpList.Add(employee);

            //}
        }
    }
}