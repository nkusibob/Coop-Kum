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
            ProjectBoard projectBoard = CreatorManager.CreateProjectBoard(); ;
            projectBoard.Manager = manager;
            projectBoard.Project = project;
            projectBoard.Employees = employees;
            return projectBoard;
        }

        internal PeopleCoop GetPeopleCoop(Coop coop, List<ProjectBoard> projectBoardList)
        {
            List<Membre> personMembre = _context.Membre
                         .Where(x => x.MyCoop == coop).ToList();
            List<OfflineMember> offlineMembers = _context.OfflineMember.Include(p => p.Person)
                       .Where(x => x.MyCoop == coop).ToList();
            PeopleCoop peopleCoop = new PeopleCoop();
            peopleCoop.PersonList = personMembre;
            peopleCoop.ProjectBoardList = projectBoardList;
            peopleCoop.OfflineMembers = offlineMembers;
            return peopleCoop;
        }
    }
}