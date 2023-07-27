using Business.Cooperative.Api.RequestModel;
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
        Task<string> BuyLivestock(Goat goat);
        Task<Goat> EatLivestock(string eatenGoatName, int idCoop);
        Task<(List<Goat> goatsToKeep, decimal totalPrice)> OptimizeHerdGrowthAsync(bool extendGenetics, int malesToKeep, bool sellGoats, decimal sellPrice, string goatName, int idCoop);
        Task<Livestock> UpdateDetails(int livestockId);
        Task<Goat> UpdateDetails(int livestockId, Goat goat);
        Task<string> BuyLivestock(string name, string genderInput, string input, decimal price, int idCoop, decimal totalPrice);
    }
}
