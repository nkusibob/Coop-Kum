using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Linq;
using System.Threading.Tasks;

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
            ViewBag.IdManager = managerId;
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("EmployeeId,Person,Salary")] Employee employee, int idPerson, int managerId)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with validation errors if the model state is invalid
                return View(employee);
            }

            // Retrieve the Person object from the database
            Employee emp = new Employee
            {
                Person = new Person()
                {
                    LastName = employee.Person.LastName,
                    FirstName = employee.Person.FirstName,
                    IdNumber = employee.Person.IdNumber,
                },


            };
            var person = employee;
            if (person == null)
            {
                // Return a 404 error if the Person object does not exist
                return NotFound();
            }

            // Assign the Person and Manager objects to the Employee
            employee.Person = emp.Person;
            employee.Manager = await _context.Manager.FindAsync(idPerson);

            try
            {
                // Add the Employee object to the context and save changes
                _context.Add(employee);
                await _context.SaveChangesAsync();

                // Redirect to the Coops Details view
                return RedirectToAction("Details", "Coops");
            }
            catch (DbUpdateException ex)
            {
                // Handle the foreign key constraint exception and return a user-friendly error message
                if (ex.InnerException is SqlException innerException && innerException.Number == 547)
                {
                    ModelState.AddModelError(string.Empty, "The Person or Manager ID specified does not exist.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving changes to the database.");
                }

                return View(employee);
            }
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