using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Web.Cooperation.Logic;

namespace Web.Cooperation.Controllers
{
    public class PeopleController : Controller
    {
        private readonly CooperativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PeopleController(CooperativeContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [Authorize]
        public IActionResult CreateForEmployee(int id)
        {
            Project project = _context.Project.Find(id);
            CoopManager coopManager = _context.Manager.Include(x => x.Project).
                                      Where(p => p.Project == project).FirstOrDefault();
            ViewBag.ManagerId = coopManager.ManagerId;

            CoopManager manager = _context.Manager.Find(coopManager.ManagerId);
            return View();
        }

        // POST: People/CreateForEmployee
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateForEmployee([Bind("PersonId,FirstName,IdNumber,LastName ,PhoneNumber,City,Country")] Person person, int ManagerId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Employees", new { id = person.PersonId, managerId = ManagerId });
            }
            return View(person);
        }

        // GET: People/Create
        [Authorize]
        public async Task<IActionResult> Create(int selectedCoopId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            ViewBag.selectedCoopId = selectedCoopId;
            int idPerson = _context.Membre
                .Include(c => c.Person)
                    .ThenInclude(p => p.CoopUser)
                .Where(x => x.Person.CoopUser == applicationUser)
                .Select(x => x.Person.PersonId)
                .FirstOrDefault();

            Membre connectedPerson = _context.Membre.Include(c => c.Person).
               ThenInclude(p => p.CoopUser).Where(x => x.Person.CoopUser == applicationUser).FirstOrDefault();

            if ((connectedPerson != null))
            {

                return RedirectToAction("Details", "Coops");
            }
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: People/Create
        
        public async Task<IActionResult> Create([Bind("PersonId,FirstName,IdNumber,LastName,PhoneNumber,City,Country")] ConnectedMember person, int selectedCoopId, int id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            person.CoopUser = applicationUser;

            var coop = _context.Coop.Find(selectedCoopId);
            if (ModelState.IsValid)
            {
                // Check if the person exists as an OfflineMember
                var existingOfflinePerson = await _context.OfflineMember
                    .Include(p => p.Person)
                    .FirstOrDefaultAsync(p =>
                        p.Person.PhoneNumber == person.PhoneNumber &&
                        p.Person.LastName == person.LastName &&
                        p.Person.FirstName == person.FirstName);

                if (existingOfflinePerson != null)
                {
                    // Person found as an OfflineMember, delete associated images in the PersonImages table
                    var associatedImages = await _context.PersonImages
                        .Where(pi => pi.PersonId == existingOfflinePerson.Person.PersonId)
                        .ToListAsync();

                    _context.PersonImages.RemoveRange(associatedImages);

                    // Delete the existing OfflineMember
                    _context.OfflineMember.Remove(existingOfflinePerson);

                    // Add the person as a new OnlineMember (Membre) with the provided data
                    _context.Membre.Add(new Membre() { Person = person, MyCoop = coop });
                }
                else
                {
                    // Person not found as an OfflineMember, check if the person is already an OnlineMember
                    var existingOnlinePerson = await _context.Membre
                        .Include(p => p.Person)
                        .FirstOrDefaultAsync(p =>
                            p.Person.PhoneNumber == person.PhoneNumber &&
                            p.Person.LastName == person.LastName &&
                            p.Person.FirstName == person.FirstName);

                    if (existingOnlinePerson != null)
                    {
                        // Person found as an OnlineMember, update the person's data
                        existingOnlinePerson.Person.FirstName = person.FirstName;
                        existingOnlinePerson.Person.LastName = person.LastName;
                        existingOnlinePerson.Person.PhoneNumber = person.PhoneNumber;
                        existingOnlinePerson.Person.City = person.City;
                        existingOnlinePerson.Person.Country = person.Country;
                        existingOnlinePerson.Person.CoopUser = applicationUser;
                        existingOnlinePerson.MyCoop = coop;
                    }
                    else
                    {
                        // Person not found as an OfflineMember or OnlineMember, add the person as a new OnlineMember (Membre)
                        _context.Membre.Add(new Membre() { Person = person, MyCoop = coop });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coops");
            }

            return View(person);
        }


        public IActionResult CreateOfflineMember(int id)
        {
            ViewBag.id = id;
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOfflineMember([Bind("PersonId,FirstName,IdNumber,LastName,idCoop")] Person person, int id)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "OfflineMember", new { id, @IdPerson = person.PersonId });
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            var membre = await _context.OfflineMember.Where(p => p.Person.PersonId == id).FirstOrDefaultAsync(); ;
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,FirstName,IdNumber,LastName,PhoneNumber,City,Country,PersonImageUrl")] Person person)
        {
            if (id.ToString() != person.PersonId.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    person.PersonId = id;
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(Convert.ToInt32(person.PersonId)))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}