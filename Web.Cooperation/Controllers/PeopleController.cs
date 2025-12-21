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
        public async Task<IActionResult> ManageUserConnection(int selectedCoopId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            ViewBag.selectedCoopId = selectedCoopId;
            Membre connectedPerson = _context.Membre.Include(c => c.Person)
                .ThenInclude(p => p.CoopUser).Where(x => x.Person.CoopUser.Id == applicationUser.Id).FirstOrDefault();

            if (connectedPerson != null)
            {
                return RedirectToAction("Details", "Coops");
            }

            // Create an instance of the ViewModel and set its properties using ApplicationUser data
            var membre = await _context.Membre
                .Include(m => m.Person)
                .FirstOrDefaultAsync(m => m.Person.CoopUserId == applicationUser.Id);



            return View(membre);

        }


        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: People/Create

        public async Task<IActionResult> ManageUserConnection( int selectedCoopId,int additionalFees)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            var coop = _context.Coop.Find(selectedCoopId);
            if (ModelState.IsValid)
            {
                // Check if the person exists as an OfflineMember
                var existingOfflinePerson = await _context.OfflineMember
                    .Include(p => p.Person)
                    .FirstOrDefaultAsync(p =>
                        p.Person.PhoneNumber == applicationUser.PhoneNumber &&
                        p.Person.LastName == applicationUser.LastName &&
                        p.Person.FirstName == applicationUser.FirstName);

                if (existingOfflinePerson != null)
                {
                    // Person found as an OfflineMember, delete associated images in the PersonImages table
                    var associatedImages = await _context.PersonImages
                        .Where(pi => pi.PersonId == existingOfflinePerson.Person.PersonId)
                        .ToListAsync();

                    _context.PersonImages.RemoveRange(associatedImages);

                    // Delete the existing OfflineMember
                    _context.OfflineMember.Remove(existingOfflinePerson);
                    var connectedMember =_context.ConnectedMember.Where(p => p.PhoneNumber== applicationUser.PhoneNumber).FirstOrDefault();
                    // Add the person as a new OnlineMember (Membre) with the provided data
                    _context.Membre.Add(Membre.Create(connectedMember,coop,additionalFees));
                }
                else
                {
                    // Person not found as an OfflineMember, check if the person is already an OnlineMember
                    var existingOnlinePerson = await _context.Membre
                        .Include(p => p.Person)
                        .FirstOrDefaultAsync(p =>
                            p.Person.PhoneNumber == applicationUser.PhoneNumber &&
                            p.Person.LastName == applicationUser.LastName &&
                            p.Person.FirstName == applicationUser.FirstName);

                    if (existingOnlinePerson != null)
                    {
                        // Person found as an OnlineMember, update the person's data
                        existingOnlinePerson.Person.UpdateFromUser(applicationUser);
                        existingOnlinePerson.Person.LinkUser(applicationUser); // si besoin

                    }
                    else
                    {

                        var connectedMember = _context.ConnectedMember.Where(p => p.PhoneNumber == existingOfflinePerson.Person.PhoneNumber).FirstOrDefault();

                        // Person not found as an OfflineMember or OnlineMember, add the person as a new OnlineMember (Membre)
                        _context.Membre.Add(Membre.Create(connectedMember,coop,additionalFees));
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coops");
            }

            return View();
        }
        // GET: People/Create
        [Authorize]
        public async Task<IActionResult> Create(int selectedCoopId)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            ViewBag.selectedCoopId = selectedCoopId;
            Membre connectedPerson = _context.Membre.Include(c => c.Person)
                .ThenInclude(p => p.CoopUser).Where(x => x.Person.CoopUser == applicationUser).FirstOrDefault();

            if (connectedPerson != null)
            {
                return RedirectToAction("Details", "Coops");
            }

            // Create an instance of the ViewModel and set its properties using ApplicationUser data
            ConnectedMember viewModel = ConnectedMember.CreateFromUser(applicationUser);


            return View(viewModel);
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
            person.UpdateFromUser (applicationUser);

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
                    _context.Membre.Add(Membre.Create(person,coop,applicationUser.Fees));
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
                        existingOnlinePerson.Person.UpdateFromUser(applicationUser);
                    

                    }
                    else
                    {
                        // Person not found as an OfflineMember or OnlineMember, add the person as a new OnlineMember (Membre)
                        _context.Membre.Add(Membre.Create(person, coop, applicationUser.Fees));
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