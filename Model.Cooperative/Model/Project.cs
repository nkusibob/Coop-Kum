using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string Name { get; set; }
        public int Efficiency { get; set; }
        public int DurationInMonth { get; set; }
        public decimal ProjectBudget { get; set; }
        public string PictureUrl { get; set; }

    }
}