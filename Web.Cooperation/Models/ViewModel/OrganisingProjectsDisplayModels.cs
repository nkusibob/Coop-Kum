using System.Collections.Generic;
using Web.Cooperation.Models.ViewModel;
using Model.Cooperative;

namespace Web.Cooperation.Models.ViewModel
{
    public record OrganisingProjectsDisplayOptions
    {
        public decimal? GlobalBenefit { get; init; }
        public decimal? Month { get; init; }
        public Coop Coop { get; init; } = null!;
        public PeopleCoop PeopleCoop { get; init; } = null!;
        // keep the tuple type returned by GetProjection
        public (List<ProjectBoard> projectBoardList, string totalEmployeeSalaryFormatted, string totalExpensesFormatted, string totalNetBenefitFormatted) ProjectionResult { get; init; }
    }

    public record OrganisingProjectsDisplayResult
    {
        public decimal InitialInvestment { get; init; }
        public decimal TotalExpenses { get; init; }
        public decimal TotalFees { get; init; }
        public decimal CurrentBalance { get; init; }
        public decimal ManagerSalary { get; init; }
        public string AccountSummary { get; init; } = string.Empty;
        public decimal TotalEmployeeSalary { get; init; }
        public decimal TotalStepBudget { get; init; }
    }


}