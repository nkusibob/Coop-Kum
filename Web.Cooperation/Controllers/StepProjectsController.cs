using Business.Cooperative;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cooperation.Controllers
{
    public class StepProjectsController : Controller
    {
        private readonly CooperativeContext _context;

        public StepProjectsController(CooperativeContext context)
        {
            _context = context;
        }

        // GET: StepProjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.StepProject.ToListAsync());
        }

        // GET: StepProjects/Details/5
        public async Task<IActionResult> Details(int? id, int projectId)
        {
            if (id == null)
            {
                return NotFound();
            }

            StepProject stepProject = await _context.StepProject
            .Include(sp => sp.Employee)
                .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(sp => sp.StepProjectId == id);

            if (stepProject == null)
            {
                return NotFound();
            }

            ViewBag.FullName = stepProject.Employee.Person.FullName;
            if (projectId< 0)
            {
                stepProject.project.ProjectId = projectId;

            }


            return View(stepProject);
        }

        // GET: StepProjects/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CreateStepProject(int employeeId)
        {
            var employee = _context.Employee.Find(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var stepProject = new StepProject()
            {
                Employee = employee,
               
            };

            return View(stepProject);
        }

        [HttpPost]
        public IActionResult CreateStepProject([FromForm] int employeeId, int projectId, [Bind("NbreOfDays,StepBuget,Description,StartingDate,VetVisit,Reviewed")] StepProject stepProject)
        {
            if (ModelState.IsValid)
            {
                // create a new StepProject object with the employee and project IDs
                stepProject.Employee =_context.Employee.Find(employeeId);

               

                // add the StepProject object to the database and save changes
                _context.StepProject.Add(stepProject);
                _context.SaveChanges();

                return RedirectToAction("Details", "Employees", new { id = employeeId });
            }

            return View(stepProject);
        }

        // POST: StepProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StepProjectId,NbreOfDays,StepBuget,Description,StartingDate,StepCategorie,Employee")] StepProject stepProject)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(stepProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stepProject);
        }

        // GET: StepProjects/Edit/5
        public async Task<IActionResult> Edit(int? id, int projectId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stepCat = await _context.StepCategories.ToListAsync();
           var img =await _context.StepProjectPicture.Where(p => p.StepProjectId==id).ToListAsync();
            StepProject stepProject = await _context.StepProject
            .Include(sp => sp.Employee)
                .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(sp => sp.StepProjectId == id);

            if (stepProject == null)
            {
                return NotFound();
            }
            ViewBag.stepCat = stepCat;
            ViewBag.Images=img;
            ViewBag.FullName = stepProject.Employee.Person.FullName;
            if (projectId < 0)
            {
                stepProject.project.ProjectId = projectId;

            }


            return View(stepProject);
        }

        // POST: StepProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NbreOfDays,StepBudget,Description,StartingDate,Reviewed,projectName,StepCategorie")] StepProject stepProject, List<IFormFile> imageFiles)
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

            // Create a list of StepProjectPicture objects from the byte arrays
            List<StepProjectPicture> stepProjectImages = new List<StepProjectPicture>();

            foreach (var imageData in imageDatas)
            {
                StepProjectPicture stepProjectImage = new StepProjectPicture
                {
                    Data = imageData,
                    StepProjectId = id
                };
                stepProjectImages.Add(stepProjectImage);
            }

            // Save the StepProjectPicture entities in the database
            _context.StepProjectPicture.AddRange(stepProjectImages);

            // Retrieve the existing StepProject object from the database
            var step = await _context.StepProject
                .Include(sp => sp.project)
                .FirstOrDefaultAsync(sp => sp.StepProjectId == id);

            // Check if the StepProject object exists
            if (step == null)
            {
                return NotFound();
            }

            // Update the properties of the StepProject object
            step.NbreOfDays = stepProject.NbreOfDays;
            step.StepBudget = stepProject.StepBudget;
            step.Description = stepProject.Description;
            step.StartingDate = stepProject.StartingDate;
            step.Reviewed = stepProject.Reviewed;
            step.StepCategorie = stepProject.StepCategorie;

            try
            {
                 _context.Update(step);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Projects", new { id = step.project.ProjectId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StepProjectExists(stepProject.StepProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: StepProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stepProject = await _context.StepProject
                .FirstOrDefaultAsync(m => m.StepProjectId == id);
            if (stepProject == null)
            {
                return NotFound();
            }

            return View(stepProject);
        }

        // POST: StepProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stepProject = await _context.StepProject.FindAsync(id);
            _context.StepProject.Remove(stepProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StepProjectExists(int id)
        {
            return _context.StepProject.Any(e => e.StepProjectId == id);
        }
    }
}