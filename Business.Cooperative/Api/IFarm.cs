using Model.Cooperative;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Cooperative.Api
{
    public interface IFarm<Livestock>
    {
        Task<List<Goat>> ListAvailableLivestock(int CoopI);
        Task<(List<(Goat, Goat)> eligiblePairs, List<Goat> pregnantGoats, string message)> BreedLivestockAsync(int idCoop);
        Task<(List<Goat> allNewBorn, List<Goat> pregnantGoats, string message)> HandleBirth(int IdCoop, List<LivestockGender> kidGenders, List<string> kidNames);
        string BuyLivestock(string name, string genderInput, string input, double price, int idCoop, ref double totalPrice);
        Task<Goat> EatLivestock(string eatenGoatName, int idCoop);
        Task<(List<Goat> goatsToKeep, double totalPrice)> OptimizeHerdGrowthAsync(bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice, string goatName, int idCoop);
    }
}
