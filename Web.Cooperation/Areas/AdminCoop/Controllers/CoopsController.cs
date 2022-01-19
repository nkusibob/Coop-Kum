using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cooperation.Areas.AdminCoop.Controllers
{
    [Area("AdminCoop")]
    public class CoopsController : Controller
    {
        private readonly CooperativeContext _context;

        public CoopsController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: AdminCoop/Coops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coop.ToListAsync());
        }

        // GET: AdminCoop/Coops/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: AdminCoop/Coops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminCoop/Coops/Create
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
                return RedirectToAction(nameof(Index));
            }
            return View(coop);
        }

        // GET: AdminCoop/Coops/Edit/5
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

        // POST: AdminCoop/Coops/Edit/5
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

        // GET: AdminCoop/Coops/Delete/5
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

        // POST: AdminCoop/Coops/Delete/5
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