using Business.Cooperative.BusinessModel;
using Business.Cooperative.Manager.ProjectStateProcessor;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Cooperative.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        //GET: api/<Project>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //GET api/<Project>/5
        [HttpGet("{id}")]

        public string Get(int id)
        {
            return "value";
        }

        //POST api/<Project>
        [HttpPost("Create")]
        [SwaggerResponse(200, "Create", typeof(Project))]
        public IManager Post([FromBody] Project project)
        {
            StepProject stp = new StepProject();
            stp.StepBuget = 30;
            stp.Description = "Foundation preapartion du sol";
            stp.NbreOfDays = 20;

            StepProject stp2 = new StepProject();
            stp2.StepBuget = 20;
            stp2.Description = "Mis en place de la fondation";
            stp2.NbreOfDays = 20;


            IManager manager = new CoopManager()
            {
                LastName = "Nkusi",
                FirstName = "Isa y'isata",
                IdNumber = 123445
            };
            manager.Project = project;
            manager.ProjectBudget = project.ProjectBudget;
            List<Employee> employees = new List<Employee>();
            Employee emp = new Employee()
            {
                IdNumber = 1111,
                FirstName = "Theo",
                LastName = "Ngarambre",
                Step = stp
            };
            emp.Salary = emp.CalculatePerStepSalary(1) * stp.NbreOfDays;
            Employee emp1 = new Employee()
            {
                IdNumber = 1111,
                FirstName = "Olivier",
                LastName = "Ngabo",
                Step = stp
            };
            emp1.Salary = emp.CalculatePerStepSalary(1) * stp.NbreOfDays;
            Employee emp2 = new Employee()
            {
                IdNumber = 1111,
                FirstName = "Collin",
                LastName = "Rumare",
                Step = stp2
            };
            Employee emp3 = new Employee()
            {
                IdNumber = 1111,
                FirstName = "Theo",
                LastName = "Cyuma",
                Step = stp
            };
            emp1.Salary = emp.CalculatePerStepSalary(1) * stp.NbreOfDays;
            emp3.Salary = emp.CalculatePerStepSalary(1) * stp.NbreOfDays;
            Employee emp4 = new Employee()

            {
                IdNumber = 1111,
                FirstName = "Jean",
                LastName = "Kizito",
                Step = stp2
            };




            emp4.Salary = emp.CalculatePerStepSalary(1) * stp.NbreOfDays;
            emp2.Salary = emp2.CalculatePerStepSalary(5) * stp.NbreOfDays;
            employees.Add(emp);
            employees.Add(emp1);
            employees.Add(emp3);
            employees.Add(emp4);
            employees.Add(emp2);
            manager.Employees = employees;
            manager.ExpenseBudget = employees.Sum(x => x.Salary);
            


           
            StepProcessor stpprc = new StepProcessor(manager);

            stpprc.StartProject();
            return manager;
        }

        //PUT api/<Project>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //DELETE api/<Project>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
