namespace Business.Cooperative.Interfaces
{
    public interface IEmployee
    {
        decimal Salary { get; set; }

        decimal CalculatePerStepSalary(int rank);
    }
}