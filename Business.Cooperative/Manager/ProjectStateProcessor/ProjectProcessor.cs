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
//        private BusinessCoopManager manager;
//        public ProjectProcessor(List<Employee> employees, BusinessCoopManager manager)
//        {
//            this.employees = employees;
//            this.manager = manager;
//        }
//        public  List<Employee> GetEmployeesForProject(BusinessProject project)
//        {
//            var context = new StepContext(new ProjectStarted(employees,manager));
//            employees = context.Request(project);
//            return employees;
//        }

//    }
//}