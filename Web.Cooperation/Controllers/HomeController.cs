using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Cooperative;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Cooperation.Models;

namespace Web.Cooperation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CooperativeContext _context;

        public HomeController(ILogger<HomeController> logger, CooperativeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        
        {
            try
            {
                ViewBag.CoopNames = await _context.Coop.Select(x => x.CoopName).ToListAsync();
            }
            catch (SqlException ex)
            {

                throw;
            }
            return View(await _context.Coop.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Affiliation()
        {
            return View(await _context.Coop.ToListAsync());
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return RedirectToAction("Details", "Coops");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}