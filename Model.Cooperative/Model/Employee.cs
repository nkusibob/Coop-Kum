using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Model.Cooperative
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; private set; }

        public decimal DailySalary { get; private set; }

        // FK Person (required)
        public int PersonId { get; private set; }
        public virtual Person Person { get; private set; } = null!;

        // FK Manager (optional depending on your flow)
        public int? ManagerId { get; private set; }
        public virtual CoopManager? Manager { get; private set; }

        // FK Step current (optional)
        public int? StepProjectId { get; private set; }
        public virtual StepProject? Step { get; private set; }

        public virtual ICollection<StepProject> Steps { get; private set; } = new List<StepProject>();

        [NotMapped]
        public int SelectedPersonId { get; set; }

        protected Employee() { } // EF

        /* ===================== FACTORY ===================== */
        public static Employee Create(Person person, decimal dailySalary, CoopManager? manager = null)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (dailySalary < 0) throw new ArgumentOutOfRangeException(nameof(dailySalary), "Daily salary cannot be negative.");

            var employee = new Employee
            {
                Person = person,
                PersonId = person.PersonId, // if person is already persisted; EF will fix otherwise
                DailySalary = dailySalary
            };

            if (manager != null)
                employee.AssignManager(manager);

            return employee;
        }

        /* ===================== BEHAVIOR ===================== */
        public void AssignManager(CoopManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            Manager = manager;
            ManagerId = manager.ManagerId;

            // keep both sides consistent (if you rely on navigation)
            if (manager.ManagedEmployees != null && !manager.ManagedEmployees.Contains(this))
                manager.ManagedEmployees.Add(this);
        }

        public void AssignStep(StepProject step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));

            if (!Steps.Contains(step))
                Steps.Add(step);

            Step = step;
            StepProjectId = step.StepProjectId;

            // If StepProject has navigation back to Employee, keep it consistent:
            if (step.Employee == null)
                step.Employee = this;
        }

        public void UpdateDailySalary(decimal dailySalary)
        {
            if (dailySalary < 0) throw new ArgumentOutOfRangeException(nameof(dailySalary));
            DailySalary = dailySalary;
        }

        public decimal CurrentEmployeeAllStepsSalary
        {
            get
            {
                var totalDays = Steps?.Sum(s => s.NbreOfDays) ?? 0;
                return DailySalary * totalDays;
            }
        }
    }
}
