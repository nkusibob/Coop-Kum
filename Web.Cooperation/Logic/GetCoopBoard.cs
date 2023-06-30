using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Cooperation.Models.ViewModel;

namespace Web.Cooperation.Logic
{
    public class GetCoopBoard
    {
        internal readonly CooperativeContext _context;
        public GetCoopBoard(CooperativeContext context)
        {
            _context = context;
        }

        internal PeopleCoop FindUserCommunity(ApplicationUser applicationUser)
        {
            Membre connectedPerson = GetConnectedMember(applicationUser);
            Coop coop = connectedPerson.MyCoop;
            Coop coopProject = GetCurrentCop(connectedPerson);
            var projectBoardList = GetProjectBoardList(coopProject);
            PeopleCoop peopleCoop = GetPeopleCoop(coop, projectBoardList.ProjectBoardList);
            peopleCoop.TotalExpected = projectBoardList.TotalBudget;
            peopleCoop.TotalExpenses = projectBoardList.ProjectBoardList.Sum(x => x.TotalStepsBudget);
            return peopleCoop;
        }
        internal (decimal TotalBudget, List<ProjectBoard> ProjectBoardList) GetProjectBoardList(Coop coopProject)
        {
            List<Project> projects = coopProject.Projects.ToList();
            decimal totalBudget = 0;
            List<ProjectBoard> projectBoardList = new List<ProjectBoard>(); // Initialize an empty list

            foreach (Project project in projects)
            {
                ProjectBoard projectBoard = GetProjectBoardAsync(project);
                if (!projectBoardList.Any(pb => pb.Project == projectBoard.Project))
                {
                    projectBoardList.Add(projectBoard);
                    totalBudget += project.ProjectBudget;
                }
            }

            return (totalBudget, projectBoardList);
        }



        internal ProjectBoard GetProjectBoardAsync(Project project)
        {
            ConnectedMember manager = _context.Manager
                .Include(x => x.Project)
                .Where(p => p.Project == project)
                .Select(x => x.Person)
                .FirstOrDefault();

            CoopManager employeeManager = _context.Manager
                .Include(x => x.ManagedEmployees)
                    .ThenInclude(me => me.Person) // include person details of managed employees
                .Include(n => n.Person)
                .FirstOrDefault(p => p.Project == project);
            GetStepsDetailsPerProject(project, employeeManager);
            employeeManager.ProjectBudget = project.ProjectBudget;

            List<Employee> employees = employeeManager.ManagedEmployees.ToList();
            MoveStepsWithValuesToCollection(employees);

            employeeManager.UpdateBudget(_context);

            ProjectBoard projectBoard = CreatorManager.CreateProjectBoard();
            projectBoard.TotalStepsBudget = employeeManager.ManagerSalary + employeeManager.AfterStepBudget;
            projectBoard.EmployeesSalary = employees.Sum(x => x.CurrentStepEmployeeSalary);
            projectBoard.Manager = manager;
            projectBoard.Project = project;
            projectBoard.Employees = employees;

            // Add the step projects associated with each employee to the Steps property of ProjectBoard
            projectBoard.Steps = employees.SelectMany(e => e.Steps).ToList();

            return projectBoard;
        }

        private void GetStepsDetailsPerProject(Project project, CoopManager employeeManager)
        {
            if (employeeManager != null)
            {
                // Get managerIds for later use
                List<int> managerIds = _context.Manager
                    .Where(m => m.Person.PersonId == employeeManager.PersonId)
                    .Select(m => m.ManagerId)
                    .ToList();

                // Get the list of step projects for the current project
                var stepProjectListCurrentProject = _context.StepProject
                    .Where(p => p.project.ProjectId == project.ProjectId)
                    .Include(e => e.Employee)
                        .ThenInclude(p => p.Person)
                    .ToList();

                // Loop through each managed employee
                foreach (var employee in employeeManager.ManagedEmployees)
                {
                    // If the employee's steps collection is null, initialize it as an empty list
                    if (employee.Steps == null)
                    {
                        employee.Steps = new List<StepProject>();
                    }
                    else
                    {
                        // Filter the employee's steps based on the project name and remove duplicates
                        employee.Steps = employee.Steps
                            .Where(s => s.project != null && s.project.Name == project.Name)
                            .Distinct()
                            .ToList();
                    }
                }

                // Add any employees from the step project list who are not already in the managed employees list
                if (managerIds.Count > 0)
                {
                    foreach (var item in stepProjectListCurrentProject)
                    {
                        if (!employeeManager.ManagedEmployees.Contains(item.Employee))
                        {
                            employeeManager.ManagedEmployees.Add(item.Employee);
                        }
                    }
                }
            }
        }

        private void MoveStepsWithValuesToCollection(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                if (employee.Steps.Count == 0 && employee.Step != null)
                {
                    var stepWithValue = employee.Step;

                    if (stepWithValue != null)
                    {
                        employee.Steps = new List<StepProject> { stepWithValue };
                        employee.Step = null;
                    }
                }
            }
        }
        internal Membre GetConnectedMember(ApplicationUser applicationUser)
        {
            Membre connectedPerson = GetCurrentUser(applicationUser);
            if (connectedPerson == null)
            {
                throw new System.Exception();
            }

            return connectedPerson;
        }

        internal Membre GetCurrentUser(ApplicationUser applicationUser)
        {
            string userEmail = applicationUser?.Id; // will give the user's Email

            Membre connectedPerson = _context.Membre
                .Include(p => p.MyCoop)
                .FirstOrDefault(p => p.Person.CoopUser.Email == applicationUser.Email);
            return connectedPerson;
        }


        internal Coop GetCurrentCop(Membre connectedPerson)
        {
            Coop coop = connectedPerson.MyCoop;
            Coop coopProject = _context.Coop.Include(x => x.Projects).Where(x => x.IdCoop == coop.IdCoop).FirstOrDefault();
            if (coop == null)
            {
                throw new System.Exception(); ;
            }

            return coopProject;
        }

       




        private ProjectBoard GetBudgetTopDipslay(CoopManager employeeManager, List<Employee> employees)
        {


            ProjectBoard projectBoard = CreatorManager.CreateProjectBoard();
            //projectBoard.EmployeesSalary = employeeStepSalaryTotal;
            projectBoard.TotalStepsBudget = employeeManager.ManagerSalary + employeeManager.AfterStepBudget;
            return projectBoard;
        }


        internal PeopleCoop GetPeopleCoop(Coop coop, List<ProjectBoard> projectBoardList)
        {
            List<Membre> OnlineMembers = GetOnlineMembers(coop);
            List<OfflineMember> offlineMembers = _context.OfflineMember.Include(p => p.Person)
                       .Where(x => x.MyCoop == coop).ToList();
            UnlistNewConnectedMember(OnlineMembers, offlineMembers);
            decimal SumFees = OnlineMembers.ToList().Sum(x => x.FeesPerYear) +
                              offlineMembers.ToList().Sum(x => x.FeesPerYear);
            PeopleCoop peopleCoop = new PeopleCoop()
            {
                SumFees = SumFees,
                PersonList = OnlineMembers,
                ProjectBoardList = projectBoardList,
                OfflineMembers = offlineMembers
            };

            return peopleCoop;
        }

        private void UnlistNewConnectedMember(List<Membre> OnlineMembers, List<OfflineMember> offlineMembers)
        {
            foreach (var onmbr in OnlineMembers.Select(p => p.Person))
            {
                var justConnectedList = offlineMembers.Select(p => p.Person).Where(p => p.IdNumber == onmbr.IdNumber);
                if (justConnectedList.Any())
                {
                    var offlinenList = _context.OfflineMember;
                    Person PersonToremove = justConnectedList.FirstOrDefault();
                    offlinenList.Remove(offlinenList.Where(p => p.Person == PersonToremove).FirstOrDefault());
                    var personList = _context.Person;
                    personList.Remove(personList.Where(p => p.IdNumber == PersonToremove.IdNumber).FirstOrDefault());
                    _context.SaveChanges();
                }

            }
        }

        private List<Membre> GetOnlineMembers(Coop coop)
        {

            List<Membre> OnlineMembers = _context.Membre.Include(p => p.Person)
                         .Where(x => x.MyCoop == coop).ToList();
            return OnlineMembers;
        }
    }
}
