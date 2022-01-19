using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Details(int? id)
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

            Person employee = _context.Employee.Where(x => x.Step == stepProject).Select(p => p.Person).FirstOrDefault();
            ViewBag.FullName = employee.FullName;
            return View(stepProject);
        }

        // GET: StepProjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StepProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StepProjectId,NbreOfDays,StepBuget,Description,ReviewDate")] StepProject stepProject)
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
        public async Task<IActionResult> Edit(int id, [Bind("StepProjectId,NbreOfDays,StepBuget,Description,ReviewDate")] StepProject stepProject)
        {
            if (id != stepProject.StepProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stepProject);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction("Index", "Coops");
            }
            return View(stepProject);
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