using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cooperation.Controllers
{
    public class StepProjectsController : Controller
    {
        private readonly CooperativeContext _context;

        public StepProjectsController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: StepProjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.StepProject.ToListAsync());
        }

        // GET: StepProjects/Details/5
        public async Task<IActionResult> Details(int? id, int projectId)
        {
            if (id == null)
            {
                return NotFound();
            }

            StepProject stepProject = await _context.StepProject
            .Include(sp => sp.Employee)
                .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(sp => sp.StepProjectId == id);

            if (stepProject == null)
            {
                return NotFound();
            }

            ViewBag.FullName = stepProject.Employee.Person.FullName;
            if (projectId< 0)
            {
                stepProject.project.ProjectId = projectId;

            }


            return View(stepProject);
        }

        // GET: StepProjects/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CreateStepProject(int employeeId)
        {
            var employee = _context.Employee.Find(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var stepProject = new StepProject()
            {
                Employee = employee,
               
            };

            return View(stepProject);
        }

        [HttpPost]
        public IActionResult CreateStepProject([FromForm] int employeeId, int projectId, [Bind("NbreOfDays,StepBuget,Description,StartingDate")] StepProject stepProject)
        {
            if (ModelState.IsValid)
            {
                // create a new StepProject object with the employee and project IDs
                stepProject.Employee =_context.Employee.Find(employeeId);

               

                // add the StepProject object to the database and save changes
                _context.StepProject.Add(stepProject);
                _context.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = employeeId });
            }

            return View(stepProject);
        }

        // POST: StepProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StepProjectId,NbreOfDays,StepBuget,Description,StartingDate,Employee")] StepProject stepProject)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(stepProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stepProject);
        }

        // GET: StepProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stepProject = await _context.StepProject.FindAsync(id);
            if (stepProject == null)
            {
                return NotFound();
            }
            ViewBag.Description = stepProject.Description;
            return View(stepProject);
        }

        // POST: StepProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StepProjectId,NbreOfDays,StepBudget,Description,StartingDate,projectName")] StepProject stepProject)
        {
            // Retrieve the existing StepProject object from the database
            var step = await _context.StepProject
                .Include(sp => sp.project)
                .FirstOrDefaultAsync(sp => sp.StepProjectId == stepProject.StepProjectId);

            // Check if the StepProject object exists
            if (step == null)
            {
                return NotFound();
            }
            //if (stepProject.project.Name == null)
            //{
            //    return NotFound();


            //}
            //int projectId = _context.Project.Where(x => x.Name == stepProject.project.Name).Select(x => x.ProjectId).FirstOrDefault();
            // Update the properties of the StepProject object except for the Employee property
            step.NbreOfDays = stepProject.NbreOfDays;
            step.StepBudget = stepProject.StepBudget;
            step.Description = stepProject.Description;
            step.StartingDate = stepProject.StartingDate;

            try
            {
                // Update the StepProject object in the database
                _context.Update(step);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Projects", new { id =step.project.ProjectId});
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StepProjectExists(stepProject.StepProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: StepProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stepProject = await _context.StepProject
                .FirstOrDefaultAsync(m => m.StepProjectId == id);
            if (stepProject == null)
            {
                return NotFound();
            }

            return View(stepProject);
        }

        // POST: StepProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stepProject = await _context.StepProject.FindAsync(id);
            _context.StepProject.Remove(stepProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StepProjectExists(int id)
        {
            return _context.StepProject.Any(e => e.StepProjectId == id);
        }
    }
}