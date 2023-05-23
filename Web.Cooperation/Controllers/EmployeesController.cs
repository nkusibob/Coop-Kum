using Business.Cooperative.BusinessModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Cooperation.Models.ViewModel;
using Employee = Model.Cooperative.Employee;
using Person = Model.Cooperative.Person;
using StepProject = Model.Cooperative.StepProject;

namespace Web.Cooperation.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly CooperativeContext _context;

        public EmployeesController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create(int id, int managerId)
        {
            ViewBag.IdPerson = id;
            ViewBag.IdManager = id;
            List<Person> existingEmployees = _context.Employee
            .Include(e => e.Person)
            .Where(e => e.Manager.PersonId == id)
            .Select(e => e.Person)
            .ToList();

            ViewBag.ExistingEmployees = new SelectList(existingEmployees, "PersonId", "FullName");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Employee/Create

        public async Task<IActionResult> Create(EmployeeViewModel model, int idPerson, string option)
        {
            Employee employee;
            var step = new StepProject
            {
                Description = model.Employee.Step.Description
            };

            _context.StepProject.Add(step);

            if (option == "Existing")
            {
                var existingEmployeeId = model.Employee.SelectedPersonId;
                var existingEmployee = await _context.Employee.FindAsync(idPerson);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                employee = existingEmployee;

                if (employee.Steps == null)
                {
                    employee.Steps = new List<StepProject>();
                }

                employee.Steps.Add(step);

                _context.Employee.Update(employee);
            }
            else if (option == "addNew")
            {
                var person = new Person
                {
                    LastName = model.Employee.Person.LastName,
                    FirstName = model.Employee.Person.FirstName,
                    IdNumber = model.Employee.Person.IdNumber,
                };

                employee = new Employee
                {
                    Salary = model.Employee.Salary,
                    Person = person,
                    Manager = await _context.Manager.FindAsync(idPerson)
                };

                if (employee.Steps == null)
                {
                    employee.Steps = new List<StepProject>();
                };
                employee.Steps.Add(step);
                _context.Employee.Add(employee);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coops");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && innerException.Number == 547)
                {
                    ModelState.AddModelError(string.Empty, "The Person or Manager ID specified does not exist.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving changes to the database.");
                }
            }

            return View(model);

        }






        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Salary")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}