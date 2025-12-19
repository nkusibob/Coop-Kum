using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cooperative.BusinessModel
{
    public class CreateEmployeeStepCommand
    {
        public int ProjectId { get; set; }

        public string Option { get; set; } // Existing | AddNew

        // Step info
        public string StepDescription { get; set; }
        public DateTime StartingDate { get; set; }
        public int NbreOfDays { get; set; }
        public decimal StepBudget { get; set; }
        public int StepCategorieId { get; set; }

        // Employee
        public decimal DailySalary { get; set; }

        // Existing employee
        public int? ExistingPersonId { get; set; }

        // New person
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? IdNumber { get; set; }
    }

}
