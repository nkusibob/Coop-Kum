using Model.Cooperative;
using System.Collections.Generic;

namespace Web.Cooperation.Models.ViewModel
{
    public class ProjectBoard
    {
        public ConnectedMember Manager { get; set; }
        public Project Project { get; set; }

        public List<StepProject> Steps { get; set; }

        public List<Employee> Employees { get; set; }

        public decimal EmployeesSalary { get; set; }
    }
}