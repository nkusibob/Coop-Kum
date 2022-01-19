using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public class CoopManager
    {
        [Key]
        public int ManagerId { get; set; }

        public int PersonId { get; set; }

        public CoopManager()
        {
            ManagedEmployee = new HashSet<Employee>();
            Person = new ConnectedMember();
            Project = new Project();
        }

        public virtual Project Project { get; set; }
        public decimal ProjectBudget { get; set; }
        public decimal ExpenseBudget { get; set; }
        public decimal AfterStepBudget { get; set; }
        public virtual ICollection<Employee> ManagedEmployee { get; set; }
        public virtual ConnectedMember Person { get; set; }
        public decimal Salary { get; set; }
    }
}