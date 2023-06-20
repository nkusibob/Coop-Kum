using Business.Cooperative;
using Business.Cooperative.Api;
using Business.Cooperative.Api.RequestModel;
using Business.Cooperative.BusinessModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Cooperation.Helper;
using Web.Cooperation.Logic;
using Web.Cooperation.Models.ViewModel;
using Project = Model.Cooperative.Project;

namespace Web.Cooperation.Controllers
{
    public class CoopsController : Controller
    {
        private readonly CooperativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GetCoopBoard getCoopBoard;
        private readonly IBusinessApiCallLogic _apiClient;
        private  string apiprojectionResponse;
        private StringBuilder responseProjection = new StringBuilder();

        public CoopsController(CooperativeContext context, UserManager<ApplicationUser> userManager, IBusinessApiCallLogic apiClient)
        {
            _context = context;
            _userManager = userManager;
            getCoopBoard = CreatorManager.CreateCoopBoard(context);
            _apiClient = apiClient;
        }

        // GET: Coops
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            PeopleCoop peopleCoop = getCoopBoard.FindUserCommunity(applicationUser);
            return View(peopleCoop);
        }
        private List<(ProjectionPerPeriod, ProjectBoard)> GetProjectionListProjects(List<Project> projects)
        {
            var projections = new List<(ProjectionPerPeriod, ProjectBoard)>();

            foreach (var project in projects)
            {
                ProjectBoard projectBoard;
                ProjectionPerPeriod projection;
                ComputingProjectionHelper.GetProjectionForCurrentProject(project, out projectBoard, out projection, getCoopBoard);
                projections.Add((projection, projectBoard));
            }

            return projections;
        }
        // GET: Coops/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string searchString, decimal? globalBenefit, decimal? month )
                
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            Model.Cooperative.Membre connectedPerson = getCoopBoard.GetCurrentUser(applicationUser);
            if (connectedPerson == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Model.Cooperative.Coop coop = getCoopBoard.GetCurrentCop(connectedPerson);
            ViewBag.id = coop.IdCoop;
            ViewData["Title"] = coop.CoopName;
            PeopleCoop peopleCoop = getCoopBoard.FindUserCommunity(applicationUser);
            List<Project> projects = peopleCoop.ProjectBoardList.Select(x=> x.Project).ToList();
            // Filter projects by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                peopleCoop.ProjectBoardList = peopleCoop.ProjectBoardList.Where(p => p.Project.Name.Contains(searchString)).ToList();
            }
            if (peopleCoop.ProjectBoardList.Any())
            {
                decimal maxDuration = (from projectBoard in peopleCoop.ProjectBoardList
                                       where projectBoard.Project != null
                                       select projectBoard.Project.DurationInMonth)
                       .Max();
                var projectionResult = await GetProjection(projects, maxDuration);
                ViewBag.simulationPeriod = $"the projected benefit for {peopleCoop.ProjectBoardList.Count.ToString("N0")} projects over {month} months is {globalBenefit:0.000}.";
                peopleCoop.ProjectBoardList = projectionResult.projectBoardList;
                decimal initialInvestment = peopleCoop.TotalExpected;
                decimal totalExpenses = peopleCoop.TotalExpenses;
                decimal totalFees = peopleCoop.SumFees;
                decimal currentBalance = coop.Budget;
                decimal MangerSalary = peopleCoop.ProjectBoardList
                .SelectMany(x => x.Employees)
                .Sum(e => e.Manager.ManagerSalary);

                
                string accountSummary = $"Account Summary for {coop.CoopName}:\n\n";
                accountSummary += $"Initial Investment: {initialInvestment.ToString("0.##")}€\n";
                accountSummary += $"Total Expenses: {totalExpenses.ToString("0.##")}€\n";
                accountSummary += $"Total Fees: {totalFees.ToString("0.##")}€\n";
                accountSummary += $"Current Balance: {currentBalance.ToString("0.##")}€\n";

                accountSummary += $"Projects Information:\n";
                accountSummary += $"- Number of Projects: {projectionResult.projectBoardList.Count}\n";
                accountSummary += $"- Number of Employees: {projectionResult.projectBoardList.Sum(x => x.Employees.Count)}\n\n";

                accountSummary += $"Financial Overview for Projects:\n";
                accountSummary += $"- Total Employee Salaries: {projectionResult.totalEmployeeSalaryFormatted}€\n";
                accountSummary += $"- Total Expenses: {projectionResult.totalExpensesFormatted}€\n";
                accountSummary += $"- Net Benefit: {projectionResult.totalNetBenefitFormatted}€\n\n";

                accountSummary += "Thank you for your continued support and involvement in our cooperative.";

                ViewBag.AccountSummary = accountSummary;
                ViewBag.AccountSummary = accountSummary;
                ViewBag.TotalInvestment = $"Initial investment: {initialInvestment.ToString("0.##")}€\n";
                ViewBag.TotalExpenses = $"Total Expenses: {totalExpenses.ToString("0.##")}€\n";
                ViewBag.TotalFees = $"Total Fees: {totalFees.ToString("0.##")}€\n";
                decimal totalBalance = (currentBalance + totalFees);
                ViewBag.CurrentBalance = $"Current Balance: {totalBalance.ToString("0.##")}€\n";
                ViewBag.NumberOfProjects = projectionResult.projectBoardList.Count;
                ViewBag.NumberOfEmployees = projectionResult.projectBoardList.Sum(x => x.Employees.Count);
                ViewBag.TotalEmployeeSalaries = projectionResult.totalEmployeeSalaryFormatted;
                ViewBag.ManagerSalary = MangerSalary.ToString("0.##");
                ViewBag.TotalProjectExpenses = projectionResult.totalExpensesFormatted;
                ViewBag.NetBenefit = $"Total net benefit: {projectionResult.totalNetBenefitFormatted}€\n" ;

            }
            int EmployeeCount = peopleCoop.ProjectBoardList
                .SelectMany(x => x.Employees)
                .Select(x => x.EmployeeId)
                .Distinct()
                .Count();
            int MemberCount = peopleCoop.OfflineMembers.Count + peopleCoop.PersonList.Count;
           
            ViewBag.CoopPitch=$"At our cooperative, we take pride in our community of members and employees, which includes {EmployeeCount:N0} dedicated employees and {MemberCount:N0} active members.";
            ViewBag.Employees = peopleCoop.ProjectBoardList.Select(x => x.Employees);




            return View(peopleCoop);
        }
        private async Task<(List<ProjectBoard> projectBoardList, string totalEmployeeSalaryFormatted, string totalExpensesFormatted, string totalNetBenefitFormatted)> GetProjection(List<Project> projects, decimal maxDuration)
        {
            List<ProjectBoard> projectBoardList = new List<ProjectBoard>();
            List<(ProjectionPerPeriod, ProjectBoard)> projectionList = GetProjectionListProjects(projects);

            decimal totalNetBenefit = 0;
            decimal totalEmployeeSalary = 0;
            decimal totalExpenses = 0;

            foreach (var (projection, projectBoard) in projectionList)
            {
                try
                {
                    ProjectProduction response = await _apiClient.CallApiProductionPlanAsync(projection);
                    List<Projection> projections = response.projectionsPerYear;
                    decimal globalBenefit = response.globalProjectedBenefit;
                    decimal netBenefit = globalBenefit - projectBoard.EmployeesSalary - (projectBoard.TotalStepsBudget - projectBoard.Project.ProjectBudget);
                    totalNetBenefit += netBenefit;
                    totalEmployeeSalary += projectBoard.EmployeesSalary;
                    totalExpenses += projectBoard.TotalStepsBudget - projectBoard.Project.ProjectBudget;
                    projectBoard.GeneratedProduction = response.projectionsPerYear.FirstOrDefault().generatedProduction;
                    projectBoardList.Add(projectBoard);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //responseProjection.AppendLine($"Overall Financial Summary:");
            //responseProjection.AppendLine($"For {projectBoardList.Count} projects ({GetProjectNames(projectBoardList)}), allocate {projectBoardList.Count} employees to tasks. The total expenses amount to {totalExpenses:0.000}€, resulting in a net benefit of {totalNetBenefit:0.000}€.");
            ViewBag.Projection = responseProjection.ToString();
            string totalEmployeeSalaryFormatted = totalEmployeeSalary.ToString("0.##");
            string totalExpensesFormatted = totalExpenses.ToString("0.##");
            string totalNetBenefitFormatted = totalNetBenefit.ToString("0.##");

            return (projectBoardList, totalEmployeeSalaryFormatted, totalExpensesFormatted, totalNetBenefitFormatted);
        }

        private string GetProjectNames(List<ProjectBoard> projectBoardList)
        {
            List<string> projectNames = projectBoardList.Select(pb => pb.Project.Name).ToList();
            return string.Join(", ", projectNames);
        }
        [HttpPost]
        public async Task<ActionResult<ProjectProduction>> GetSimulation(SimulationRequestModel model)
        {
            var projects = new List<Business.Cooperative.BusinessModel.BusinessProject>();

            string[] inputArray = model.ProjectList.Split(',');
            List<int> intList = new List<int>();
            foreach (string s in inputArray)
            {
                intList.Add(int.Parse(s));
            }

            try
            {
                foreach (var projectId in intList)
                {
                    var project = await _context.Project
                        .Where(p => p.ProjectId == projectId)
                        .Select(p => new { p.Name, p.Efficiency, p.DurationInMonth, p.ProjectBudget, p.PictureUrl })
                        .FirstOrDefaultAsync();

                    if (project == null)
                    {
                        return NotFound();
                    }

                    var projectObject = new Business.Cooperative.BusinessModel.BusinessProject
                    {
                        Name = project.Name,
                        Efficiency = project.Efficiency,
                        DurationInMonth = project.DurationInMonth,
                        ProjectBudget = project.ProjectBudget,
                        PictureUrl = project.PictureUrl
                    };
                    projects.Add(projectObject);
                }

                Goal goal = new Goal
                {
                    GoalToReach = model.GoalToReach,
                    Projects = projects
                };

                ProjectProduction result = await _apiClient.CallApiSimulationAsync(goal);
                int numProjects = result.projectionsPerYear.Count;
                decimal simulationperiodinMonth = result.projectionsPerYear.FirstOrDefault().numberOfMonth;
                int months = (int)(simulationperiodinMonth % 12);
                int years = (int)simulationperiodinMonth / 12;
                var sentenceDetails = result.projectionsPerYear.Select(x => new { x.projectName, x.generatedProduction }).ToList();

                string sentence;
                if (months == 0)
                {
                    sentence = $"Based on current running projects, we anticipate that it will take {years} years to reach this target benefit.\n";
                }
                else
                {
                    sentence = $"Based on current running projects, we anticipate that it will take {years} years and {months} months to reach this target benefit.\n";
                }

                // Add sentence details
                sentence += " \n";
                for (int i = 0; i < sentenceDetails.Count; i++)
                {
                    sentence += $"\n{sentenceDetails[i].projectName}: {sentenceDetails[i].generatedProduction.ToString("0.##")} €\n";
                    if (i < sentenceDetails.Count - 1)
                    {
                        sentence += ", ";
                    }
                }

                return Json(new { Sentence = sentence });


            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes.

                // Return a BadRequest response with an error message.
                return BadRequest($"Failed to get simulation: {ex.Message}");
            }
        }        // GET: Coops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoopStartViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new coop object using the form values
                var coop = new Model.Cooperative.Coop
                {
                    CoopName = model.Coop.CoopName,
                    Budget = model.Coop.Budget
                };
                ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
                applicationUser.FirstName = model.Membre.Person.FirstName;
                applicationUser.LastName = model.Membre.Person.LastName;
                //person.CoopUser = applicationUser;
                // Create a new member object using the form values
                var membre = new Model.Cooperative.Membre
                {
                    Person = new ConnectedMember
                    {
                        FirstName = model.Membre.Person.FirstName,
                        LastName = model.Membre.Person.LastName,
                        IdNumber = model.Membre.Person.IdNumber,
                        CoopUser = applicationUser // Set this property to null as it is not used
                    },
                    FeesPerYear = model.Membre.FeesPerYear
                };

                // Add the new member to the membres list of the coop object
                coop.Membres.Add(membre);

                // Add the new coop to the context and save changes
                _context.Add(coop);
                await _context.SaveChangesAsync();

                // Redirect to the "Create" action of the "People" controller, passing the ID of the new coop
                return RedirectToAction("Create", "People", new { id = coop.IdCoop });
            }

            return View();
        }


        // GET: Coops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _context.Coop.FindAsync(id);
            if (coop == null)
            {
                return NotFound();
            }
            return View(coop);
        }

        // POST: Coops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCoop,CoopName,Budget")] Model.Cooperative.Coop coop)
        {
            if (id != coop.IdCoop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoopExists(coop.IdCoop))
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
            return View(coop);
        }

        // GET: Coops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coop = await _context.Coop
                .FirstOrDefaultAsync(m => m.IdCoop == id);
            if (coop == null)
            {
                return NotFound();
            }

            return View(coop);
        }

        // POST: Coops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coop = await _context.Coop.FindAsync(id);
            _context.Coop.Remove(coop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoopExists(int id)
        {
            return _context.Coop.Any(e => e.IdCoop == id);
        }
    }
}