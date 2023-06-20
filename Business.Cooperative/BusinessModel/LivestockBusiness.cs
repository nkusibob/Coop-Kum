using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.BusinessModel
{
    public abstract class LivestockBusiness
    {

        public virtual string Sell(Livestock livestock, double price)
        {
            livestock.IsSold = true;
            livestock.Price = price;
            return $"{livestock.Name} has been sold for {price:C}.";
        }

        public virtual string KillAnimal(Livestock livestock)
        {
            if (!livestock.IsAlive)
            {
                return $"{livestock.Name} is already dead.";
            }

            livestock.IsAlive = false;
            NotifyObservers(livestock);
            return $"{livestock.Name} is dead.";
        }

        protected virtual string NotifyObservers(Livestock livestock)
        {
            var message = $"{livestock.Name} has died.";

            return message;
            // Implement notification logic here, such as sending notifications to observers in an ASP.NET Core application.
            // You can use any notification mechanism appropriate for your application.
            // This method can be overridden in derived classes to provide specific notification logic.
        }

        
    }
}
