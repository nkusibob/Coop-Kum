using Business.Cooperative.Interfaces;

namespace Business.Cooperative.BusinessModel
{
    public class BusinessMembre : BusinessPerson, IMembre
    {
        public BusinessMembre()
        {
            MyCoop = new BusinessCoop();
        }

        public virtual BusinessCoop MyCoop { get; set; }

        public decimal MemberShipFees { get; set; }
    }
}