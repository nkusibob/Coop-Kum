

namespace Tests.Employee
{
    using System;
    using Model.Cooperative;
    using Xunit;

    public class EmployeeDomaintest
    {
        [Fact]
        public void Create_WithNegativeSalary_Throws()
        {
            var p = new Person
            {
                FirstName = "A",
                LastName = "B",
                City = "X",
                Country = "Y",
                PhoneNumber = "1",
                PersonImageUrl = ""
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => Employee.Create(p, -1m));
        }
    }

}
