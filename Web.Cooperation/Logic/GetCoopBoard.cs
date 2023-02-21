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
            List<ProjectBoard> projectBoardList = GetProjectBoardList(coopProject);
            PeopleCoop peopleCoop = GetPeopleCoop(coop, projectBoardList);
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
            Membre connectedPerson = _context.Membre.Include(p => p.MyCoop).Where(x => x.Person.CoopUser == applicationUser).FirstOrDefault();
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

        internal List<ProjectBoard> GetProjectBoardList(Coop coopProject)
        {
            List<Project> projects = coopProject.Projects.ToList();
            List<ProjectBoard> projectBoardList = CreatorManager.CreateProjectBoardList();
            foreach (Project project in projects)
            {
                ProjectBoard projectBoard = GetProjectBoard(project);
                projectBoardList.Add(projectBoard);
            }

            return projectBoardList;
        }

        internal ProjectBoard GetProjectBoard(Project project)
        {
            ConnectedMember manager = _context.Manager.Include(x => x.Project).
            Where(p => p.Project == project).Select(x => x.Person).FirstOrDefault();
            CoopManager employeeManager = _context.Manager.Include(x => x.ManagedEmployee).
              Where(p => p.Project == project).Include(n => n.Person).FirstOrDefault();
            List<Employee> employees = employeeManager.ManagedEmployee.ToList();
            ProjectBoard projectBoard = AdjustingBudget(employeeManager, employees);
            projectBoard.Manager = manager;
            projectBoard.Project = project;
            projectBoard.Employees = employees;
            return projectBoard;
        }

        private ProjectBoard AdjustingBudget(CoopManager employeeManager, List<Employee> employees)
        {
            var stepsCost = _context.Employee.Include(x => x.Step).
                            Where(x => x.Manager.Person.PersonId == employeeManager.PersonId);
            ProjectBoard projectBoard = CreatorManager.CreateProjectBoard();
            projectBoard.EmployeesSalary = employees.ToList().Sum(x => x.Salary);
            employeeManager.ExpenseBudget += stepsCost.ToList().Sum(x => x.Step.StepBuget);
            employeeManager.ExpenseBudget += projectBoard.EmployeesSalary;
            employeeManager.AfterStepBudget = employeeManager.ProjectBudget - employeeManager.ExpenseBudget;
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
            ConnectedMember FirstOnline = _context.Person.Join(_context.Membre,
                      pers => pers.PersonId,
                      mbr => mbr.Person.PersonId,
                      (pers, mbr) => pers).FirstOrDefault() as ConnectedMember;
            List<Membre> OnlineMembers = _context.Membre.Include(p => p.Person)
                         .Where(x => x.MyCoop == coop).ToList();
            OnlineMembers.FirstOrDefault().Person = FirstOnline;
            return OnlineMembers;
        }
    }
}