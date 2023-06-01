using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System;
using System.Linq;
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
        public async Task<IActionResult> CreateAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
           
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
        public async Task<IActionResult> Create([Bind("PersonId,FirstName,IdNumber,LastName,idCoop,PhoneNumber,City,Country")] ConnectedMember person, int id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            applicationUser.FirstName = person.FirstName;
            applicationUser.LastName = person.LastName;
            person.CoopUser = applicationUser;
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Membres", new { id, @IdPerson = person.PersonId });
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
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,FirstName,IdNumber,LastName")] ApplicationUser person)
        {
            if (id.ToString() != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(Convert.ToInt32(person.Id)))
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