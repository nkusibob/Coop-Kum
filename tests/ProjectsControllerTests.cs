using System.Threading.Tasks;
using Business.Cooperative.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Web.Cooperation.Controllers;
using Xunit;
using Model.Cooperative;

namespace Web.Cooperation.Tests
{
    public class ProjectsControllerTests
    {
        private CooperativeContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CooperativeContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new CooperativeContext(options);
        }

        [Fact]
        public async Task GetSimulation_WhenApiThrowsHttpRequestException_Returns502()
        {
            var context = CreateInMemoryContext("testdb1");
            context.Project.Add(new Project { ProjectId = 2001, Name = "Test", Efficiency = 5, DurationInMonth = 12, ProjectBudget = 100 });
            await context.SaveChangesAsync();

            var apiMock = new Mock<IBusinessApiCallLogic>();
            apiMock.Setup(x => x.CallApiSimulationAsync(It.IsAny<Business.Cooperative.Goal>()))
                   .ThrowsAsync(new System.Net.Http.HttpRequestException("upstream failed"));

            var controller = new ProjectsController(context, null, apiMock.Object, null, NullLogger<ProjectsController>.Instance);

            var result = await controller.GetSimulation(2001);

            Assert.IsType<ObjectResult>(result.Result);
            var obj = result.Result as ObjectResult;
            Assert.Equal(StatusCodes.Status502BadGateway, obj.StatusCode);
        }

        [Fact]
        public async Task GetSimulation_WhenApiReturnsNullResult_Returns502()
        {
            var context = CreateInMemoryContext("testdb2");
            context.Project.Add(new Project { ProjectId = 2002, Name = "Test", Efficiency = 5, DurationInMonth = 12, ProjectBudget = 100 });
            await context.SaveChangesAsync();

            var apiMock = new Mock<IBusinessApiCallLogic>();
            apiMock.Setup(x => x.CallApiSimulationAsync(It.IsAny<Business.Cooperative.Goal>()))
                   .ReturnsAsync((Business.Cooperative.ProjectProduction)null);

            var controller = new ProjectsController(context, null, apiMock.Object, null, NullLogger<ProjectsController>.Instance);

            var result = await controller.GetSimulation(2002);

            Assert.IsType<ObjectResult>(result.Result);
            var obj = result.Result as ObjectResult;
            Assert.Equal(StatusCodes.Status502BadGateway, obj.StatusCode);
        }
    }
}
