
using System;

namespace Model.Cooperative
{
    public class Membre : BasicMember
    {
        protected Membre() { } // EF Core needs it

        // ⚠️ On garde private set public pour l'instant (transition)
        public virtual ConnectedMember Person { get; private set; } = null!;
        public virtual Coop MyCoop { get;  set; } = null!;

        public string GrandparentTag { get;  set; } = "";
        public string Country { get;  set; } = "";
        public string PhoneNumber { get;  set; } = "";
        public string City { get;  set; } = "";
        public string Town { get;  set; } = "";

        public static Membre Create(ConnectedMember person, Coop coop, decimal feesPerYear)
        {
            ArgumentNullException.ThrowIfNull(person);
            ArgumentNullException.ThrowIfNull(coop);
            ArgumentOutOfRangeException.ThrowIfNegative(feesPerYear);

            return new Membre
            {
                Person = person,
                MyCoop = coop,
                FeesPerYear = feesPerYear,

                // Copier les champs “duplicatés” si tu veux garder ce design pour l’instant
                City = person.City ?? "",
                Country = person.Country ?? "",
                PhoneNumber = person.PhoneNumber ?? ""
            };
        }

        // Optionnel: pour sync si tu continues à dupliquer
        public void SyncContactFromPerson()
        {
            City = Person?.City ?? City;
            Country = Person?.Country ?? Country;
            PhoneNumber = Person?.PhoneNumber ?? PhoneNumber;
        }
    }
}
