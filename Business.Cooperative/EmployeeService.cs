
using Business.Cooperative.BusinessModel;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cooperative
{
    public class EmployeeService
    {
        private readonly CooperativeContext _context;

        public EmployeeService(CooperativeContext context)
        {
            _context = context;
        }

        public async Task CreateEmployeeWithStepAsync(CreateEmployeeStepCommand cmd)
        {
            var project = await _context.Project.FindAsync(cmd.ProjectId)
                ?? throw new InvalidOperationException("Project not found");

            // 1️⃣ Create StepProject
            var step = new Model.Cooperative.StepProject
            {
                Description = cmd.StepDescription,
                ProjectId = project.ProjectId,
                StartingDate = cmd.StartingDate,
                NbreOfDays = cmd.NbreOfDays,
                StepBudget = cmd.StepBudget,
                StepCategorieId = cmd.StepCategorieId
            };

            _context.StepProject.Add(step);
            await _context.SaveChangesAsync();

            // 2️⃣ Resolve employee
            Model.Cooperative.Employee employee;

            if (cmd.Option == "Existing")
            {
                employee = await _context.Employee
                    .Include(e => e.Steps)
                    .Include(e => e.Person)
                    .FirstOrDefaultAsync(e => e.Person.PersonId == cmd.ExistingPersonId)
                    ?? throw new InvalidOperationException("Employee not found");
            }
            else
            {
                var person = new Person
                {
                    FirstName = cmd.FirstName!,
                    LastName = cmd.LastName!,
                    IdNumber = cmd.IdNumber!.Value
                };

                employee = Model.Cooperative.Employee.Create(person, cmd.DailySalary);

                var manager = await _context.Manager
                    .FirstOrDefaultAsync(m => m.Project.ProjectId == project.ProjectId);

                if (manager != null)
                    employee.AssignManager(manager);

                _context.Employee.Add(employee);
            }

            // 3️⃣ Attach step
            employee.AssignStep(step);
           

            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeCreateDataDto> GetEmployeeCreateDataAsync(int projectId, int managerId)
        {
            var manager = await _context.Manager
                .Include(m => m.Project)
                .Include(m => m.ManagedEmployees)
                    .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(m => m.ManagerId == managerId);

            if (manager == null)
                throw new InvalidOperationException("Manager not found.");

            if (manager.Project == null || manager.Project.ProjectId != projectId)
                throw new InvalidOperationException("Manager not assigned to this project.");

            var employees = manager.ManagedEmployees
                .Select(e => new EmployeeDto(
                    e.Person.PersonId,
                    e.Person.FullName))
                .ToList();

            var categories = await _context.StepCategories
                .Select(c => new StepCategoryDto(c.Id, c.Name))
                .ToListAsync();

            return new EmployeeCreateDataDto
            {
                ProjectId = projectId,
                ManagerId = managerId,
                ExistingEmployees = employees,
                StepCategories = categories
            };
        }



    }

   
}

   

