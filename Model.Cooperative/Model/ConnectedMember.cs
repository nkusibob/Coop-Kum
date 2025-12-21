
    using System;

    namespace Model.Cooperative
    {
        public class ConnectedMember : Person
        {
            protected ConnectedMember() { } // EF

            public string? CoopUserId { get; private set; }
            public virtual ApplicationUser? CoopUser { get; private set; }

            public static ConnectedMember CreateFromUser(ApplicationUser user)
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                if (string.IsNullOrWhiteSpace(user.Id)) throw new ArgumentException("User.Id required", nameof(user));

                return new ConnectedMember
                {
                    CoopUser = user,
                    CoopUserId = user.Id,

                    // Person fields (required by DB)
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    City = user.City ?? "",
                    Country = user.Country ?? "",
                    PersonImageUrl = "" // IMPORTANT if NOT NULL in DB
                };
            }

            public void LinkUser(ApplicationUser user)
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                CoopUser = user;
                CoopUserId = user.Id;
            }

            public void UpdateFromUser(ApplicationUser user)
            {
                if (user == null) throw new ArgumentNullException(nameof(user));

                FirstName = user.FirstName ?? FirstName;
                LastName = user.LastName ?? LastName;
                PhoneNumber = user.PhoneNumber ?? PhoneNumber;
                City = user.City ?? City;
                Country = user.Country ?? Country;
            }
        }
    }

