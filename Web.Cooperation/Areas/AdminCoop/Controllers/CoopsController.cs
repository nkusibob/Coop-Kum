using Business.Cooperative.Contracts.Coop;
using Business.Cooperative.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Cooperative;
using System.Threading.Tasks;
using System.Linq;

namespace Web.Cooperation.Areas.AdminCoop.Controllers
{
    [Area("AdminCoop")]
    public class CoopsController : Controller
    {
        private readonly ICoopService _coopService;

        public CoopsController(ICoopService coopService)
        {
            _coopService = coopService;
        }

        // GET: AdminCoop/Coops
        public async Task<IActionResult> Index()
        {
            var coops = await _coopService.GetAllAsync();
            return View(coops.Select(c => new Coop { IdCoop = c.IdCoop, CoopName = c.CoopName, Budget = c.Budget }).ToList());
        }

        // GET: AdminCoop/Coops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _coopService.GetByIdAsync(id.Value);
            if (coop == null)
            {
                return NotFound();
            }

            return View(new Coop { IdCoop = coop.IdCoop, CoopName = coop.CoopName, Budget = coop.Budget });
        }

        // GET: AdminCoop/Coops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminCoop/Coops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCoop,CoopName,Budget")] Coop coop)
        {
            if (!ModelState.IsValid)
            {
                return View(coop);
            }

            await _coopService.CreateAsync(new CreateCoopRequest(coop.CoopName, coop.Budget));
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminCoop/Coops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _coopService.GetByIdAsync(id.Value);
            if (coop == null)
            {
                return NotFound();
            }

            return View(new Coop { IdCoop = coop.IdCoop, CoopName = coop.CoopName, Budget = coop.Budget });
        }

        // POST: AdminCoop/Coops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCoop,CoopName,Budget")] Coop coop)
        {
            if (id != coop.IdCoop)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(coop);
            }

            var updated = await _coopService.UpdateAsync(id, new UpdateCoopRequest(coop.CoopName, coop.Budget));
            if (updated == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminCoop/Coops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _coopService.GetByIdAsync(id.Value);
            if (coop == null)
            {
                return NotFound();
            }

            return View(new Coop { IdCoop = coop.IdCoop, CoopName = coop.CoopName, Budget = coop.Budget });
        }

        // POST: AdminCoop/Coops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _coopService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
