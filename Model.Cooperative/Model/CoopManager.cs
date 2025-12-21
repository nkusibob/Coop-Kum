using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Model.Cooperative
{
    public class CoopManager
    {
        [Key]
        public int ManagerId { get; private set; }

        public int PersonId { get; private set; }
        public virtual ConnectedMember Person { get; private set; } = null!;

        public int ProjectId { get; private set; }
        public virtual Project Project { get; private set; } = null!;

        public decimal ProjectBudget { get; private set; }
        public decimal ManagerSalary { get; private set; }

        public virtual List<Employee> ManagedEmployees { get; private set; } = new();

        [NotMapped]
        public decimal? ExpenseBudget { get; private set; }

        private decimal _afterStepBudget;

        [NotMapped]
        public decimal AfterStepBudget => _afterStepBudget;

        protected CoopManager() { } // EF

        public static CoopManager Create(ConnectedMember person, Project project, decimal projectBudget, decimal managerSalary)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (projectBudget < 0) throw new ArgumentOutOfRangeException(nameof(projectBudget));
            if (managerSalary < 0) throw new ArgumentOutOfRangeException(nameof(managerSalary));

            return new CoopManager
            {
                Person = person,
                PersonId = person.PersonId,

                Project = project,
                ProjectId = project.ProjectId,

                ProjectBudget = projectBudget,
                ManagerSalary = managerSalary
            };
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));
            if (!ManagedEmployees.Contains(employee))
                ManagedEmployees.Add(employee);
        }
        public void AssignProject(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            Project = project;
            ProjectId = project.ProjectId;
        }
        public void UpdateBudget()
        {
            ManagedEmployees.RemoveAll(e => e == null);

            var salaryTotal = ManagedEmployees.Sum(x => x.CurrentEmployeeAllStepsSalary);
            var stepBudgetTotal = ManagedEmployees.Sum(x => x.Steps?.Sum(s => s.StepBudget) ?? 0);

            ExpenseBudget = salaryTotal + stepBudgetTotal;
            _afterStepBudget = ProjectBudget - (ExpenseBudget ?? 0m);
        }
    }
}
