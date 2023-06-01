using Business.Cooperative;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Cooperative.Api;
using Web.Cooperation.Logic;
using Web.Cooperation.Models.ViewModel;
using System.Buffers.Text;
using Microsoft.CodeAnalysis;
using Project = Model.Cooperative.Project;
using Web.Cooperation.Helper;

namespace Web.Cooperation.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly CooperativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GetCoopBoard getCoopBoard;
        private const string URL = "https://sub.domain.com/objects.json";
        private readonly IBusinessApiCallLogic _apiClient;

        public ProjectsController(CooperativeContext context, UserManager<ApplicationUser> userManager,IBusinessApiCallLogic apiClient)
        {
            _context = context;
            _userManager = userManager;
            getCoopBoard = CreatorManager.CreateCoopBoard(context);
            _apiClient = apiClient;

        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

       // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id, decimal globalBenefit,decimal? month)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            CoopManager coopManager = _context.Manager
               .Include(x => x.Project)
               .Include(x => x.ManagedEmployees)
                   .ThenInclude(e => e.Steps)
               .Where(p => p.Project == project)
               .FirstOrDefault();

            var managers = _context.Manager
                .Where(m => m.Person.PersonId == coopManager.PersonId)
                .ToList();

            if (managers.Any() && coopManager.ManagedEmployees.Count == 0)
            {
                managers = managers.Where(m => m.ManagerId != coopManager.ManagerId).ToList();

                //foreach (var manager in managers)
                //{
                //    var managerEmployees = _context.Manager
                //        .Include(x => x.Project)
                //        .Include(x => x.ManagedEmployees)
                //            .ThenInclude(e => e.Steps)
                //        .Where(p => p.Person.PersonId == manager.Person.PersonId && p.Project == project)
                //        .FirstOrDefault();

                //    // Access the steps for each managed employee of the manager
                //    foreach (var employee in managerEmployees.ManagedEmployees)
                //    {
                //        var steps = employee.Steps;
                //        // Do something with the steps...
                //    }
                //}
            }


            ViewBag.ManagerId = coopManager.ManagerId;
            ViewBag.ProjectId = id;

            if (month != null) {
                ViewBag.simulationPeriod = $"Note: Your project efficiency suggest {month} months to achieve your desired goal :{globalBenefit:0.000} , based on your initial budget.";
            };
            ProjectBoard projectBoard = await GetProjection(project);
            //foreach (var emp in projectBoard.Employees)
            //{
            //    var step = _context.StepProject.Find(emp.StepProjectId);
            //    emp.Steps = new List<StepProject>();
            //    emp.Steps.Add(step);
            //}
            projectBoard.GeneratedProduction = globalBenefit;
            return View(projectBoard);

            
        }
        [HttpPost]
        public async Task<ActionResult<ProjectProduction>> GetSimulation(int? projectId)
        {
            try
            {
                var project = await _context.Project
                    .Where(p => p.ProjectId == projectId)
                    .Select(p => new { p.Name, p.Efficiency, p.DurationInMonth, p.ProjectBudget, p.PictureUrl })
                    .FirstOrDefaultAsync();

                if (project == null)
                {
                    return NotFound();
                }

                var projectObject = new Business.Cooperative.BusinessModel.Project
                {
                    Name = project.Name,
                    Efficiency = project.Efficiency,
                    DurationInMonth = project.DurationInMonth,
                    ProjectBudget = project.ProjectBudget,
                    PictureUrl = project.PictureUrl
                };

                var projects = new List<Business.Cooperative.BusinessModel.Project> { projectObject };

                decimal goalToReach = decimal.Parse(Request.Form["goalToReach"]);

                Goal goal = new Goal
                {
                    GoalToReach = goalToReach,
                    Projects = projects
                };

                ProjectProduction result = await _apiClient.CallApiSimulationAsync(goal);
                var simulationperiodinMonth = result.projectionsPerYear.FirstOrDefault().numberOfMonth;
                int months = (int)(simulationperiodinMonth % 12);
                int years = (int)simulationperiodinMonth / 12;

                string sentence;
                if (months == 0)
                {
                    sentence = $"Based on the financial data and efficiency of the selected project, we anticipate that it will take {years} years to reach your target benefit of {Math.Round(result.globalProjectedBenefit, 2)}€.";
                }
                else
                {
                    sentence = $"Based on the financial data and efficiency of the selected project, we anticipate that it will take {years} years and {months} months to reach your target benefit of {Math.Round(result.globalProjectedBenefit, 2)}€.";
                }

                return Json(new { Sentence = sentence });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.

                // Return a BadRequest response with an error message.
                return BadRequest($"Failed to get simulation: {ex.Message}");
            }

        }

        private async Task<ProjectBoard> GetProjection(Project project)
        {
            ProjectBoard projectBoard;
            ProjectionPerPeriod projection;
           
            GetProjectionForCurrentProject(project, out projectBoard, out projection);

            try
            {
                var sb = new StringBuilder();

                ProjectProduction response = await _apiClient.CallApiProductionPlanAsync(projection);
                List<Projection> projections = response.projectionsPerYear;
                decimal globalBenefit = response.globalProjectedBenefit;
                decimal totalExpenses = projectBoard.EmployeesSalary + ( projectBoard.TotalStepsBudget -projectBoard.Project.ProjectBudget) ;
                decimal netBenefit = globalBenefit - totalExpenses;

                string firstName = projectBoard.Manager.FirstName;
                string lastName = projectBoard.Manager.LastName;

                sb.AppendLine($"Based on the efficiency of the project and duration of {projections.FirstOrDefault().numberOfMonth.ToString("F0")} months, as validated by the Manager : {firstName} {lastName}, the projected production for {projections.FirstOrDefault().projectName} is {globalBenefit.ToString("F0")}€, with total expenses of {totalExpenses.ToString("F0")}€, resulting in a net benefit of {netBenefit.ToString("F0")}€. Thank you for your continued support of our project!");

                

                ViewBag.Projection = sb.ToString();
                ViewBag.GeneratedProduction = globalBenefit.ToString("F0");
                ViewBag.numberOfMonth = projections.FirstOrDefault().numberOfMonth.ToString("F0");
            }
            catch (Exception)
            {

                throw;
            }







            return projectBoard;
        }
        

        private void GetProjectionForCurrentProject(Project project, out ProjectBoard projectBoard, out ProjectionPerPeriod projection)
        {
            ComputingProjectionHelper.GetProjectionForCurrentProject(project, out projectBoard,out projection, getCoopBoard );
        }

        // GET: Projects/Create
        public IActionResult Create(int id)
        {
            var coop = _context.Coop.Find(id);
            List<ConnectedMember> connectedMembers = _context.Membre.Include(x => x.Person)
                       .Where(x => x.MyCoop == coop).Select(p => p.Person).ToList();
            ViewData["FullName"] = new SelectList(connectedMembers, "PersonId", "FullName");
            //ViewData["Employee"] = new SelectList(EmployeeList, "PersonId", "FullName");
            ViewBag.idCoop = id;
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Efficiency,DurationInMonth,ProjectBudget")] Project project,ProjectManager pm  ,int IdCoop)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
                Membre connectedPerson = getCoopBoard.GetCurrentUser(applicationUser);
                if (connectedPerson == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (IdCoop == 0)
                {
                    return NotFound();
                }

                var coop = await _context.Coop.FindAsync(IdCoop);
                if (coop == null)
                {
                    return NotFound();
                }
              

                StartProject(project, pm.CoopManager, coop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coops");
            }
            return View(project);
        }

      

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Efficiency,DurationInMonth,ProjectBudget,PictureUrl")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
        private void StartProject(Project project, CoopManager  coopManager, Coop coop)
        {


            coopManager.UpdateBudget(_context);
            // Retrieve the ConnectedMember object for the CoopManager
            coopManager.Person = _context.ConnectedMember.Find(coopManager.PersonId);

            // Assign the project to the CoopManager
            coopManager.Project = project;

            // Add the CoopManager to the database
            _context.Manager.Add(coopManager);
            // Add the project to the Coop's list of projects
            coop.Projects.Add(project);
            _context.Coop.Update(coop);
            _context.SaveChanges();














            //int SelectedValue = pm.CoopManager.PersonId;
            //pm.CoopManager.Person = _context.ConnectedMember.Find(SelectedValue);
            //pm.CoopManager.Project = project;
            //_context.Add(pm.CoopManager);
            //Coop coop = _context.Coop.Find(IdCoop);
            //coop.Projects.Add(project);
            //_context.Add(project);
            //_context.Update(coop);
        }
    }
}