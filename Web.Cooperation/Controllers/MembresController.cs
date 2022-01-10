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
    public class MembresController : Controller
    {
        private readonly CooperativeContext _context;
        private int pkPerson;
        public MembresController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: Membres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Membre.ToListAsync());
        }

        // GET: Membres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membre
                .FirstOrDefaultAsync(m => m.MembreId == id);
            if (membre == null)
            {
                return NotFound();
            }

            return View(membre);
        }

        // GET: Membres/Create
        public IActionResult Create(int IdPerson)
        {
            ViewBag.IdPerson = IdPerson;
            return View();
        }

        // POST: Membres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembreId,FeesPerYear")] Membre membre ,int id, int IdPerson)
        {
            if (ModelState.IsValid)
            {
                var coop = await _context.Coop.FindAsync(id);
                var person = await _context.Person.FindAsync(IdPerson);
                if (person == null)
                {
                    return NotFound();
                }
                if (coop == null)
                {
                    return NotFound();
                }
                membre.MyCoop = coop;
                membre.Person = person;
                _context.Add(membre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membre);
        }

        // GET: Membres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membre.FindAsync(id);
            if (membre == null)
            {
                return NotFound();
            }
            return View(membre);
        }

        // POST: Membres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembreId,FeesPerYear")] Membre membre)
        {
            if (id != membre.MembreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembreExists(membre.MembreId))
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
            return View(membre);
        }

        // GET: Membres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membre
                .FirstOrDefaultAsync(m => m.MembreId == id);
            if (membre == null)
            {
                return NotFound();
            }

            return View(membre);
        }

        // POST: Membres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membre = await _context.Membre.FindAsync(id);
            _context.Membre.Remove(membre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembreExists(int id)
        {
            return _context.Membre.Any(e => e.MembreId == id);
        }
    }
}
