using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Cooperative;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cooperation.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        public async Task<IActionResult> AssignAdminRoleAsync()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            await _userManager.AddToRoleAsync(applicationUser, "CoopAdmin");
            return RedirectToAction("Create", "Coops");
        }
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);

            return RedirectToAction("Index");
        }
    }
}
