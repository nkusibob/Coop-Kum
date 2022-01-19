using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cooperation.Controllers
{
    public class CoopManagersController : Controller
    {
        private readonly CooperativeContext _context;

        public CoopManagersController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: CoopManagers
        public async Task<IActionResult> Index()
        {
            var cooperativeContext = _context.Manager.Include(c => c.Person);
            return View(await cooperativeContext.ToListAsync());
        }

        // GET: CoopManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coopManager = await _context.Manager
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (coopManager == null)
            {
                return NotFound();
            }

            return View(coopManager);
        }

        // GET: CoopManagers/Create
        public ActionResult Create(int id, int projectId)
        {
            var coop = _context.Coop.Find(id);
            List<ConnectedMember> connectedMembers = _context.Membre.Include(x => x.Person)
                       .Where(x => x.MyCoop == coop).Select(p => p.Person).ToList();

            //var EmployeeList = _context.Employee.Select(x => x.Person);
            //List<Decimal> prjBudget = new List<Decimal>();
            //prjBudget.Add(_context.Project.Find(projectId).ProjectBudget);

            ViewData["FullName"] = new SelectList(connectedMembers, "PersonId", "FullName");
            //ViewData["Employee"] = new SelectList(EmployeeList, "PersonId", "FullName");
            ViewData["ProjectBudget"] = _context.Project.Find(projectId).ProjectBudget;
            ViewBag.projectId = projectId;
            ViewBag.idCoop = id;

            return View();

            //return RedirectToAction("Create", "Project", new { id });
        }

        // POST: CoopManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerId,PersonId,ProjectBudget,ExpenseBudget,AfterStepBudget,Salary")] CoopManager coopManager, int id, int projectId)
        {
            if (ModelState.IsValid)
            {
                int SelectedValue = coopManager.PersonId;
                coopManager.Person = _context.ConnectedMember.Find(SelectedValue);
                coopManager.Project = _context.Project.Find(projectId);
                _context.Add(coopManager);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coops");
            }
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", coopManager.PersonId);
            return View(coopManager);
        }

        // GET: CoopManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coopManager = await _context.Manager.FindAsync(id);
            if (coopManager == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", coopManager.PersonId);
            return View(coopManager);
        }

        // POST: CoopManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerId,PersonId,ProjectBudget,ExpenseBudget,AfterStepBudget,Salary")] CoopManager coopManager)
        {
            if (id != coopManager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coopManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoopManagerExists(coopManager.ManagerId))
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
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", coopManager.PersonId);
            return View(coopManager);
        }

        // GET: CoopManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coopManager = await _context.Manager
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (coopManager == null)
            {
                return NotFound();
            }

            return View(coopManager);
        }

        // POST: CoopManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coopManager = await _context.Manager.FindAsync(id);
            _context.Manager.Remove(coopManager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoopManagerExists(int id)
        {
            return _context.Manager.Any(e => e.ManagerId == id);
        }
    }
}