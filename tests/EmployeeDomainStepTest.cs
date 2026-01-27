using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class EmployeeDomainStepTest
    {
        [Fact]
        public void AssignStep_SetsCurrentStep_AndAddsToSteps()
        {
            var p = new Person { FirstName = "A", LastName = "B", City = "X", Country = "Y", PhoneNumber = "1", PersonImageUrl = "" };
            var e = Model.Cooperative.Employee.Create(p, 100);

            var step = new StepProject { StepProjectId = 123, NbreOfDays = 2 };

            e.AssignStep(step);

            Assert.Equal(123, e.StepProjectId);
            Assert.NotNull(e.Step);
            Assert.Contains(step, e.Steps);
        }
    }
}

  
