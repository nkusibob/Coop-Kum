//using Business.Cooperative.BusinessModel;
//using Business.Cooperative.ProjectState.ProjectStep;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Business.Cooperative.Manager.ProjectStateProcessor
//{
//    public  class  ProjectProcessor
//    {
//        private List<Employee> employees;
//        private CoopManager manager;
//        public ProjectProcessor(List<Employee> employees, CoopManager manager)
//        {
//            this.employees = employees;
//            this.manager = manager;
//        }
//        public  List<Employee> GetEmployeesForProject(Project project)
//        {
//            var context = new StepContext(new ProjectStarted(employees,manager));
//            employees = context.Request(project);
//            return employees;
//        }

//    }
//}
