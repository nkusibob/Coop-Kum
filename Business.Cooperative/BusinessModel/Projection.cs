using System.Text.Json.Serialization;

namespace Business.Cooperative
{
    public class Projection
    {
      
        public string projectName { get; set; }

      
        public decimal generatedProduction { get; set; }

        public decimal numberOfMonth { get; set; }
    }
}