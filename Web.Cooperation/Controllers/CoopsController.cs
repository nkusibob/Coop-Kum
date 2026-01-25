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
using System.Globalization;
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
            Membre connectedPerson = getCoopBoard.GetCurrentUser(applicationUser);
            if (connectedPerson == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Coop coop = getCoopBoard.GetCurrentCop(connectedPerson);
            ViewBag.id = coop.IdCoop;
            ViewData["Title"] = coop.CoopName + " Coop";
            PeopleCoop peopleCoop = getCoopBoard.FindUserCommunity(applicationUser);

            FilteringSteps(peopleCoop);

            List<Project> projects = peopleCoop.ProjectBoardList.Select(x => x.Project).ToList();
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
               

                // Build options and call the refactored method
                var displayOptions = new OrganisingProjectsDisplayOptions
                {
                    GlobalBenefit = globalBenefit,
                    Month = month,
                    Coop = coop,
                    PeopleCoop = peopleCoop,
                    ProjectionResult = projectionResult
                };
                
                OrganisingProjectsDisplayResult displayResult = OrganisingProjectsDisplay(displayOptions);

                peopleCoop.Computed.ManagerSalary = displayResult.ManagerSalary;
                peopleCoop.Computed.TotalEmployeeSalary = displayResult.TotalEmployeeSalary;
                peopleCoop.Computed.TotalStepBudget = displayResult.TotalStepBudget;

                peopleCoop.Computed.GrandTotal =
                    displayResult.ManagerSalary + displayResult.TotalEmployeeSalary + displayResult.TotalStepBudget;

                peopleCoop.Computed.NumberOfEmployees = projectionResult.projectBoardList
                    .SelectMany(pb => pb.Employees)
                    .Distinct()
                    .Count();

                peopleCoop.Computed.TotalProjectExpensesFormatted = projectionResult.totalExpensesFormatted;
                peopleCoop.Computed.NetBenefitFormatted = projectionResult.totalNetBenefitFormatted;

                // only if you want budget inside peopleCoop for the view:
                peopleCoop.Computed.CurrentBalance = coop.Budget;
                peopleCoop.Computed.TotalBalance = coop.Budget + peopleCoop.SumFees;

            }
            int EmployeeCount = peopleCoop.ProjectBoardList
                .SelectMany(x => x.Employees)
                .Select(x => x.EmployeeId)
                .Distinct()
                .Count();
            var expenseByCategory = peopleCoop.ProjectBoardList
              .SelectMany(projectBoard => projectBoard.Steps)
              .GroupBy(stepProject => stepProject.StepCategorie?.Name ?? "Uncategorized")
              .Select(categoryGroup => new
              {
                  Category = categoryGroup.Key,
                  Total = categoryGroup.Sum(stepProject => stepProject.StepCategorieId == 3 ? stepProject.StepBudget + stepProject.Employee.DailySalary : stepProject.StepBudget)
              })
              .ToDictionary(x => x.Category, x => x.Total);
            var expenseByCategoryAndDate = peopleCoop.ProjectBoardList
           .SelectMany(projectBoard => projectBoard.Steps)
           .GroupBy(stepProject => new { Category = stepProject.StepCategorie?.Name ?? "Uncategorized", Month = stepProject.ReviewDate.ToString("MMMM yyyy") })
           .Select(group => new ExpenseByCategoryAndDateModel
           {
               Category = group.Key.Category,
               Month = group.Key.Month,
               Total = group.Sum(stepProject => stepProject.StepCategorieId == 3 ? stepProject.StepBudget + stepProject.Employee.DailySalary : stepProject.StepBudget)
           })
           .ToList();

            var soldGoats = _context.Livestock
                .Where(goat => goat.IsSold && goat.SellDate != null)
                .AsEnumerable()
                .GroupBy(goat => goat.SellDate.Value.ToString("MMMM yyyy"))
                .Select(group => new
                {
                    SellDate = group.Key,
                    TotalPrice = group.Sum(goat => goat.Price)
                })
                .ToDictionary(x => x.SellDate, x => x.TotalPrice);

            // Group the expenseByCategoryAndDate list by month
            var expenseByMonth = expenseByCategoryAndDate.GroupBy(item => item.Month);

            // Calculate net profit by month (including year in the key)
            var netProfitByMonthList = new List<(DateTime MonthYear, decimal NetProfit)>();

            foreach (var group in expenseByMonth)
            {
                var monthWithYear = group.Key;
                var totalExpenses = group.Sum(item => item.Total);





                var totalRevenue = soldGoats.ContainsKey(monthWithYear) ? soldGoats[monthWithYear] : 0;
                var netProfit = totalRevenue - totalExpenses;
                var dateTime = DateTime.ParseExact(monthWithYear, "MMMM yyyy", CultureInfo.InvariantCulture);

                // Add to the list
                netProfitByMonthList.Add((dateTime, netProfit));

            }

            // Now you have the net profit list, and you can convert it to a dictionary if needed
            var netProfitByMonth = netProfitByMonthList.ToDictionary(x => x.MonthYear.ToString("MMMM yyyy"), x => x.NetProfit);










            // Compute total employee salary
            var totalEmployeeSalary = peopleCoop.ProjectBoardList
           .SelectMany(projectBoard => projectBoard.Steps)
           .Sum(stepProject => stepProject.Employee.DailySalary * stepProject.NbreOfDays);
            ViewBag.netProfitByMonth = netProfitByMonth;
            ViewBag.soldLivestock = soldGoats;
            ViewBag.expenseByCategoryAndDate = expenseByCategoryAndDate;
            // Add total employee salary as "Salary" category
            expenseByCategory.Add("Salary", totalEmployeeSalary);




            int MemberCount = peopleCoop.OfflineMembers.Count + peopleCoop.PersonList.Count;

            ViewBag.CoopPitch = $"At our cooperative, we take pride in our community of members and employees, which includes {EmployeeCount:N0} dedicated employees and {MemberCount:N0} active members.";
            ViewBag.Employees = peopleCoop.ProjectBoardList.Select(x => x.Employees);
            GoatService goatService = new GoatService();
            var goats = peopleCoop.Livestocks.Where(livestock => livestock.LivestockType == "Goat" && !livestock.IsSold).ToList();
            // Calculate the food quantity and shelter size
            decimal foodQuantity = goatService.CalculateFoodQuantity(goats);

            decimal shelterSize = goatService.CalculateShelterSize(goats);

            // Calculate meat production and manure production based on the number of goats
            decimal totalWeight = goats.Sum(goat => goat.Weight);
            decimal meatProduction = goatService.CalculateMeatProduction(totalWeight);
            decimal manureProduction = goatService.CalculateManureProduction(goats.Count);
            int numberOfYears = 1;
            decimal mortalityPercentage = 5;
            decimal averageKidsPerBirth = 2;

            // Populate the ViewBag with the results
            int maleAdultCount = goats.Count(goat => goat.Gender == LivestockGender.Male && goat.Age >= 12); ;
            int femaleAdultCount = goats.Count(goat => goat.Gender == LivestockGender.Female && goat.Age >= 12);
            int maleLambCount = goats.Count(goat => goat.Gender == LivestockGender.Male && goat.Age < 12);
            int femaleLambCount = goats.Count(goat => goat.Gender == LivestockGender.Female && goat.Age < 12);
            int projectedGoatCount = goatService.CalculateProjectedGoatCount(femaleAdultCount, maleAdultCount, numberOfYears, mortalityPercentage, averageKidsPerBirth);


            var goatInformation = new List<string>
                    {
                        $"For {maleAdultCount}  adult male goats",
                        $"{femaleAdultCount}  adult female goats",
                        $"{maleLambCount} male lambs",
                        $"{femaleLambCount} female lambs",
                        $"you will need to plan {foodQuantity} Kg/day of food",
                        $"The shelter size should be {shelterSize} m²",
                        $"The expected meat production is {meatProduction:0.##} Kg",
                        $"and you can expect a manure production of {manureProduction} Kg per year"
                    };

            var projectedGoatCountInfo = $"Based on the provided data of an initial count of {maleAdultCount + femaleAdultCount} goats, including a mortality rate of {mortalityPercentage}%, and an average of {averageKidsPerBirth} kids per birth, the projected goat count after {numberOfYears} year is estimated to be {projectedGoatCount} goats";

            ViewBag.GoatInformation = goatInformation;
            ViewBag.ProjectedGoatCountInfo = projectedGoatCountInfo;
            var img = _context.LivestockImages.Where(g => !g.Livestock.IsSold).ToList();
            ViewBag.Images = img;
            var personImage = _context.PersonImages.Include(x => x.Person).ToList();
            ViewBag.PersonImage = personImage;
            ViewBag.expenseByCategory = expenseByCategory;







            return View(peopleCoop);
        }


        private  void FilteringSteps(PeopleCoop peopleCoop)
        {
            // First, find the incorrect Steps and store them in a list
            var incorrectSteps = new List<Model.Cooperative.StepProject>();
            foreach (var projectBoard in peopleCoop.ProjectBoardList)
            {
                var incorrectStepsInProject = projectBoard.Steps
                    .Where(step => step.ProjectId != projectBoard.Project.ProjectId)
                    .ToList();

                incorrectSteps.AddRange(incorrectStepsInProject);
            }

            // Next, remove the incorrect Steps from the wrong projects
            foreach (var projectBoard in peopleCoop.ProjectBoardList)
            {
                projectBoard.Steps.RemoveAll(step => incorrectSteps.Contains(step));
            }

            // Now, associate the correct Steps with their respective Projects
            foreach (var step in incorrectSteps)
            {
                var correctProjectBoard = peopleCoop.ProjectBoardList
                    .FirstOrDefault(pb => pb.Project.ProjectId == step.ProjectId);

                if (correctProjectBoard != null)
                {
                    correctProjectBoard.Steps.Add(step);
                }
            }
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
                catch (Exception ex)
                {
                    throw ;
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

        // New/refactored method signature (move the old OrganisingProjectsDisplay implementation here).
        private OrganisingProjectsDisplayResult OrganisingProjectsDisplay(OrganisingProjectsDisplayOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.PeopleCoop == null) throw new ArgumentNullException(nameof(options.PeopleCoop));
            if (options.Coop == null) throw new ArgumentNullException(nameof(options.Coop));
            if (options.PeopleCoop.ProjectBoardList == null) throw new ArgumentNullException(nameof(options.PeopleCoop.ProjectBoardList));

            ViewBag.simulationPeriod =
                $"the projected benefit for {options.PeopleCoop.ProjectBoardList.Count.ToString("N0")} projects over {options.Month} months is {options.GlobalBenefit:0.000}.";

            // If ProjectionResult or projectBoardList can be null, guard it:
            ViewBag.projectionResult = options.ProjectionResult.projectBoardList;

            // Compute local values (declare variables!)
            decimal initialInvestment = options.PeopleCoop.InitialInvestiment;
            decimal totalExpenses = options.PeopleCoop.TotalExpenses;
            decimal totalFees = options.PeopleCoop.SumFees;
            decimal currentBalance = options.Coop.Budget;

            decimal managerSalary = options.PeopleCoop.ProjectBoardList
                .Where(pb => pb?.Employees != null)
                .SelectMany(pb => pb.Employees)
                .Where(e => e?.Manager != null)
                .Sum(e => e.Manager.ManagerSalary);

            decimal totalEmployeeSalary= options.PeopleCoop.ProjectBoardList
                .Where(pb => pb?.Steps != null)
                .SelectMany(pb => pb.Steps)
                .Where(step => step?.Employee != null)
                .Sum(step => (decimal)step.NbreOfDays * step.Employee.DailySalary);

            decimal totalStepBudget = options.PeopleCoop.ProjectBoardList
                .Where(pb => pb?.Steps != null)
                .SelectMany(pb => pb.Steps)
                .Sum(step => step.StepBudget);

            // Build account summary (declare it!)
            string accountSummary = $"Account Summary for {options.Coop.CoopName}:\n\n";
            accountSummary += $"Initial Investment: {initialInvestment:0.##}€\n";
            accountSummary += $"Total Expenses: {totalExpenses:0.##}€\n";
            accountSummary += $"Total Fees: {totalFees:0.##}€\n";
            accountSummary += $"Current Balance: {currentBalance:0.##}€\n\n";

            accountSummary += $"Projects Information:\n";
            accountSummary += $"- Number of Projects: {options.PeopleCoop.ProjectBoardList.Count}\n";
            accountSummary += $"- Number of Employees: {options.PeopleCoop.ProjectBoardList.Sum(x => x.Employees?.Count ?? 0)}\n\n";

            accountSummary += $"Financial Overview for Projects:\n";
            accountSummary += $"- Total Employee Salaries: {options.ProjectionResult.totalEmployeeSalaryFormatted ?? "N/A"}€\n";
            accountSummary += $"- Total Expenses: {options.ProjectionResult.totalExpensesFormatted ?? "N/A"}€\n";
            accountSummary += $"- Net Benefit: {options.ProjectionResult.totalNetBenefitFormatted ?? "N/A"}€\n\n";

            accountSummary += "Thank you for your continued support and involvement in our cooperative.";

            // IMPORTANT: Initialize the result object before setting properties
            var result = new OrganisingProjectsDisplayResult
            {
                InitialInvestment = initialInvestment,
                TotalExpenses = totalExpenses,
                TotalFees = totalFees,
                CurrentBalance = currentBalance,
                ManagerSalary = managerSalary,
                AccountSummary = accountSummary,
                TotalEmployeeSalary = totalEmployeeSalary,
                TotalStepBudget = totalStepBudget
            };

            return result;
        }

    }
}