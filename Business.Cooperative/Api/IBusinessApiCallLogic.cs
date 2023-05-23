using System;
using System.Threading.Tasks;

namespace Business.Cooperative.Api
{
    public interface IBusinessApiCallLogic
    {
        Task<ProjectProduction> CallApiSimulationAsync(Goal goal);
        Task<ProjectProduction> CallApiProductionPlanAsync(ProjectionPerPeriod prjperiod);

    }
}
