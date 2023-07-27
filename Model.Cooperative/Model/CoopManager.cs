using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Model.Cooperative
{
    public class CoopManager
    {
        [Key]
        public int ManagerId { get; set; }
        public int PersonId { get; set; }

        public CoopManager()
        {
            ManagedEmployees = new List<Employee>();
            Person = new ConnectedMember();
            Project = new Project();
        }

        public virtual Project Project { get; set; }
        public decimal ProjectBudget { get; set; }

        [NotMapped]
        public decimal? ExpenseBudget { get; set; }

        private decimal _afterStepBudget;
        [NotMapped]
        public decimal AfterStepBudget
        {
            get { return _afterStepBudget; }
            private set { _afterStepBudget = value; }
        }

        public virtual List<Employee> ManagedEmployees { get; set; }
        public virtual ConnectedMember Person { get; set; }
        public decimal ManagerSalary { get; set; }

        public void UpdateBudget(CooperativeContext context)
        {
            ManagedEmployees.RemoveAll(employee => employee == null);
            var allEmployees_StepSalaryTotal = ManagedEmployees.Sum(x => x.CurrentEmployeeAllStepsSalary);
            var currentProjectStepBudgetTotal = ManagedEmployees.Sum(x => x.Steps?.Sum(p => p.StepBudget) ?? 0);

            ExpenseBudget = allEmployees_StepSalaryTotal + currentProjectStepBudgetTotal;
            _afterStepBudget = (decimal)(ProjectBudget + ExpenseBudget);
        }
    }

}