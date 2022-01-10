using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;

namespace Web.Cooperation.Controllers
{
    public class CoopsController : Controller
    {
        private readonly CooperativeContext _context;

        public CoopsController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: Coops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coop.ToListAsync());
        }

        // GET: Coops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.id = id;
            Coop coop = await _context.Coop.Include(x=> x.Membres).ThenInclude(t=>t.Person)
                .FirstOrDefaultAsync(m => m.IdCoop == id);
            if (coop == null)
            {
                return NotFound();
            }
            List<Membre> membres =await _context.Membre
                         .Where(x=>x.MyCoop ==coop).ToListAsync();
            var emp = membres.Select(x => x.Person).ToList();
            return View(emp);
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
                return RedirectToAction("Create", "People",new { id = coop.IdCoop });
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
