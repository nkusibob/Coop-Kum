using System;
using Model.Cooperative;
using Xunit;



namespace Tests
{
    

    public class ConnectedMemberDomainTests
    {
        [Fact]
        public void CreateFromUser_MapsRequiredFields()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "A",
                LastName = "B",
                City = "X",
                Country = "Y",
                PhoneNumber = "1"
            };

            var cm = ConnectedMember.CreateFromUser(user);

            Assert.Equal(user.Id, cm.CoopUserId);
            Assert.Equal("A", cm.FirstName);
            Assert.Equal("X", cm.City);
        }
    }

}
