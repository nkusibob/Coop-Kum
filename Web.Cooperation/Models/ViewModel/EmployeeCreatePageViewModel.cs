using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Web.Cooperation.Models.ViewModel
{
    public class EmployeeCreatePageViewModel
    {
        public int ProjectId { get; set; }
        public int ManagerId { get; set; }

        public EmployeeViewModel Employee { get; set; } = new EmployeeViewModel();

        public List<SelectListItem> ExistingEmployees { get; set; } = new();
        public List<SelectListItem> StepCategories { get; set; } = new();
    }

}
