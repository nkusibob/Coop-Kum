using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Collections.Generic;
using System.Linq;
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

        internal (decimal TotalBudget, List<ProjectBoard> ProjectBoardList) GetProjectBoardList(Coop coopProject)
        {
            List<Project> projects = coopProject.Projects.ToList();
            decimal totalBudget = 0;
            List<ProjectBoard> projectBoardList = CreatorManager.CreateProjectBoardList();
            foreach (Project project in projects)
            {
                ProjectBoard projectBoard = GetProjectBoard(project);
                projectBoardList.Add(projectBoard);
                totalBudget += project.ProjectBudget;
            }

            return (totalBudget, projectBoardList);
        }


        internal ProjectBoard GetProjectBoard(Project project)
        {
            ConnectedMember manager = _context.Manager.Include(x => x.Project).
            Where(p => p.Project == project).Select(x => x.Person).FirstOrDefault();
            CoopManager employeeManager = _context.Manager
               .Include(x => x.ManagedEmployees)
                   .ThenInclude(me => me.Person) // include person details of managed employees
               .Include(n => n.Person)
               .Include(e => e.ManagedEmployees)
                   .ThenInclude(m => m.Steps) // include steps of managed employees
               .FirstOrDefault(p => p.Project == project);


            employeeManager.ProjectBudget = project.ProjectBudget;
            List<Employee> employees = employeeManager.ManagedEmployees;
            employeeManager.UpdateBudget(_context);
            ProjectBoard projectBoard = CreatorManager.CreateProjectBoard();
            projectBoard.TotalStepsBudget = employeeManager.Salary + employeeManager.AfterStepBudget;
            projectBoard.EmployeesSalary = employees.Sum(x => x.CurrentStepEmployeeSalary);
            projectBoard.Manager = manager;
            projectBoard.Project = project;
            projectBoard.Employees = employees;
            return projectBoard;
        }

        private ProjectBoard GetBudgetTopDipslay(CoopManager employeeManager, List<Employee> employees)
        {
          
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            ProjectBoard projectBoard = CreatorManager.CreateProjectBoard();
            //projectBoard.EmployeesSalary = employeeStepSalaryTotal;
            projectBoard.TotalStepsBudget = employeeManager.Salary + employeeManager.AfterStepBudget;
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
                SumFees =SumFees,
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