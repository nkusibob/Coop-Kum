using Model.Cooperative.Model;

namespace Business.Cooperative.BusinessModel
{
    public class EmployeeCreateDataDto
    {
        public int ProjectId { get; init; }
        public int ManagerId { get; init; }

        public List<EmployeeDto> ExistingEmployees { get; init; } = new();
        public List<StepCategoryDto> StepCategories { get; init; } = new();
    }


}