using System.Collections.Generic;
using Web.Cooperation.Models.ViewModel;

namespace Model.Cooperative
{
    public class PeopleCoop
    {
        public decimal SumFees { get; set; }
        public decimal InitialInvestiment { get; set; }

        public decimal TotalExpenses { get; set; }
        public List<Membre> PersonList { get; set; }
        public List<ProjectBoard> ProjectBoardList { get; set; }
        public List<OfflineMember> OfflineMembers { get; set; }
        public List<Livestock> Livestocks { get; set; }
        public PeopleCoopComputed Computed { get; set; } = new();
    }

    public class PeopleCoopComputed
    {
        public decimal ManagerSalary { get; set; }
        public decimal TotalEmployeeSalary { get; set; }
        public decimal TotalStepBudget { get; set; }
        public decimal GrandTotal { get; set; }

        public int NumberOfEmployees { get; set; }

        public string? TotalProjectExpensesFormatted { get; set; }
        public string? NetBenefitFormatted { get; set; }

        // If you need coop budget in the view but don't want to pass Coop:
        public decimal CurrentBalance { get; set; }
        public decimal TotalBalance { get; set; }
    }
    public class ExpenseByCategoryAndDateModel
    {
        public string Category { get; set; }
        public string Month { get; set; }
        public decimal Total { get; set; }
    }



}