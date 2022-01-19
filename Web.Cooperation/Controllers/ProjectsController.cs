using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Cooperation.Logic;
using Web.Cooperation.Models.ViewModel;

namespace Web.Cooperation.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly CooperativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GetCoopBoard getCoopBoard;

        public ProjectsController(CooperativeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            getCoopBoard = CreatorManager.CreateCoopBoard(context);

        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

       // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            CoopManager coopManager = _context.Manager.Include(x => x.Project).
               Where(p => p.Project == project).FirstOrDefault();
            ViewBag.ManagerId = coopManager.ManagerId;
            ViewBag.ProjectId = id;
            ProjectBoard projectBoard = getCoopBoard.GetProjectBoard(project);
            return View(projectBoard);
        }

        // GET: Projects/Create
        public IActionResult Create(int id)
        {
            ViewBag.idCoop = id;

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Efficiency,DurationInMonth,ProjectBudget")] Project project, int id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
                Membre connectedPerson = getCoopBoard.GetCurrentUser(applicationUser);
                if (connectedPerson == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Coop coop = _context.Coop.Find(id);
                coop.Projects.Add(project);
                _context.Add(project);
                _context.Update(coop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "CoopManagers", new { id = coop.IdCoop, projectId = project.ProjectId });
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Efficiency,DurationInMonth,ProjectBudget")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Coops");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}