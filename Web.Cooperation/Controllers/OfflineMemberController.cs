using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using Model.Cooperative.Migrations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cooperation.Controllers
{
    public class OfflineMemberController : Controller
    {
        private readonly CooperativeContext _context;

        public OfflineMemberController(CooperativeContext context)
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
        public async Task<IActionResult> Create([Bind("MembreId,FeesPerYear,Person")] OfflineMember membre, int id)
        {
            if (ModelState.IsValid)
            {
                var coop = await _context.Coop.FindAsync(id);
                var idCoop = _context.Membre.Include(x => x.MyCoop).Select(d => d.MyCoop);
               
                if (coop == null)
                {
                    return NotFound();
                }
                coop.Budget += membre.FeesPerYear;
                membre.MyCoop = coop;
               
                _context.Add(membre);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coops", new { id });
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
            var membre = await _context.OfflineMember
            .Include(o => o.Images)
                .ThenInclude(p => p.Person)
            .Include(o => o.Person) // Include the Person entity of the OfflineMember
            .Where(o => o.Person.PersonId == id)
            .FirstOrDefaultAsync();


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


        public async Task<IActionResult> Edit(int id, [Bind("MembreId,FeesPerYear,Images,Person,BusinessPerson")] OfflineMember membre, List<IFormFile> imageFiles)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    List<byte[]> imageDatas = new List<byte[]>();

                    foreach (var imageFile in imageFiles)
                    {
                        byte[] imageData = null;
                        if (imageFile != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await imageFile.CopyToAsync(memoryStream);
                                imageData = memoryStream.ToArray();
                            }
                        }
                        imageDatas.Add(imageData);
                    }

                    // Retrieve the existing offline member from the database
                    var existingMembre = await _context.OfflineMember
                        .Include(o => o.Images)
                        .ThenInclude(p => p.Person)
                        .Include(o => o.Person)
                        .FirstOrDefaultAsync(o => o.Person.PersonId == id);

                    if (existingMembre == null)
                    {
                        return NotFound();
                    }

                    // Update the person image URL and other properties
                    existingMembre.Person.PersonImageUrl = membre.Person.PersonImageUrl;
                    existingMembre.Person.FirstName = membre.Person.FirstName;
                    existingMembre.Person.LastName = membre.Person.LastName;
                    existingMembre.Person.PhoneNumber = membre.Person.PhoneNumber;
                    existingMembre.FeesPerYear = membre.FeesPerYear;
                    existingMembre.Person.City = membre.Person.City;
                    existingMembre.Person.Country = membre.Person.Country;

                    // Update other properties as needed

                    // Add or update the member images
                    foreach (var imageData in imageDatas)
                    {
                        PersonPicture memberImage = new PersonPicture
                        {
                            Data = imageData,
                            PersonId = id
                        };
                        existingMembre.Images.Add(memberImage);
                    }

                    _context.Update(existingMembre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!MembreExists(membre.MembreId))
                    {
                        ModelState.AddModelError("", "An error occurred while updating this person's details: " + ex.Message);
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Coops");

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