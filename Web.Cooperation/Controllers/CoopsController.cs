using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Linq;
using System.Threading.Tasks;
using Web.Cooperation.Logic;

namespace Web.Cooperation.Controllers
{
    public class CoopsController : Controller
    {
        private readonly CooperativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GetCoopBoard getCoopBoard;

        public CoopsController(CooperativeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            getCoopBoard = CreatorManager.CreateCoopBoard(context);
        }

        // GET: Coops
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            PeopleCoop peopleCoop = getCoopBoard.FindUserCommunity(applicationUser);
            return View(peopleCoop);
        }

        // GET: Coops/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string searchString)
                
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            Membre connectedPerson = getCoopBoard.GetCurrentUser(applicationUser);
            if (connectedPerson == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Coop coop = getCoopBoard.GetCurrentCop(connectedPerson);
            ViewBag.id = coop.IdCoop;
            ViewData["Title"] = coop.CoopName;
            PeopleCoop peopleCoop = getCoopBoard.FindUserCommunity(applicationUser);
            // Filter projects by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                peopleCoop.ProjectBoardList = peopleCoop.ProjectBoardList.Where(p => p.Project.Name.Contains(searchString)).ToList();
            }
            return View(peopleCoop);
        }

        // GET: Coops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCoop,CoopName,Budget")] Coop coop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "People", new { id = coop.IdCoop });
            }
            return View(coop);
        }

        // GET: Coops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _context.Coop.FindAsync(id);
            if (coop == null)
            {
                return NotFound();
            }
            return View(coop);
        }

        // POST: Coops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCoop,CoopName,Budget")] Coop coop)
        {
            if (id != coop.IdCoop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoopExists(coop.IdCoop))
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
            return View(coop);
        }

        // GET: Coops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _context.Coop
                .FirstOrDefaultAsync(m => m.IdCoop == id);
            if (coop == null)
            {
                return NotFound();
            }

            return View(coop);
        }

        // POST: Coops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coop = await _context.Coop.FindAsync(id);
            _context.Coop.Remove(coop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoopExists(int id)
        {
            return _context.Coop.Any(e => e.IdCoop == id);
        }
    }
}