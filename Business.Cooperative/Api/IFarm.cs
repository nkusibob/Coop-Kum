using Model.Cooperative;
using System.Collections.Generic;

namespace Business.Cooperative.Api
{
    public interface IFarm<Livestock>
    {
        List<Livestock> ListAvailableLivestock(int CoopI);
        void BreedLivestock(int kidCount, LivestockGender kidGender, string kidName,int idCoop);
        string BuyLivestock(string name, string genderInput, string input, double price,int idCoop, ref double totalPrice);
        void EatLivestock(string eatenGoatName);
        string OptimizeHerdGrowth(bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice,string goatName);
    }

}
