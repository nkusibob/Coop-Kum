using Business.Cooperative.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cooperative.Interfaces
{
    public interface IApiClientNonStatic
    {
        Task CreateEmployeasync(CreateEmployeeStepCommand employeeDTo);
        Task<EmployeeCreateDataDto> GetEmployeeCreateDataAsync(int projectId, int managerId);
    }
}
