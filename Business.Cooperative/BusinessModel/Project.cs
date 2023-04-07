namespace Business.Cooperative.BusinessModel
{
    public class Project
    {
        public string Name { get; set; }
        public int Efficiency { get; set; }
        public int DurationInMonth { get; set; }
        public decimal ProjectBudget { get; set; }
        public string? PictureUrl { get; set; }
    }
}