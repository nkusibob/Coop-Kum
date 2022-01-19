using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative
{
    public abstract class BasicMember
    {
        [Key]
        public int MembreId { get; set; }

        public decimal FeesPerYear { get; set; } = 10;
    }
}