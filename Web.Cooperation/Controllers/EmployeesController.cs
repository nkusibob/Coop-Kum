using Business.Cooperative;
using Business.Cooperative.Api;
using Business.Cooperative.BusinessModel;
using Business.Cooperative.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Cooperation.Models.ViewModel;
using CoopManager = Model.Cooperative.CoopManager;
using Employee = Model.Cooperative.Employee;
using Person = Model.Cooperative.Person;
using StepProject = Model.Cooperative.StepProject;

namespace Web.Cooperation.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IApiClientNonStatic _apiClient;

        public EmployeesController(IApiClientNonStatic apiClient)
        {
            _apiClient = apiClient;
        }

        //// GET: Employees
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Employee.ToListAsync());
        //}

        //// GET: Employees/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employee
        //        .FirstOrDefaultAsync(m => m.EmployeeId == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(employee);
        //}
        [Authorize(Policy = "RequireBoardRole")]
        // GET: Employees/Create
        public async Task<IActionResult> Create(int projectId, int managerId)
        {
            var data = await _apiClient.GetEmployeeCreateDataAsync(projectId, managerId);

            var vm = new EmployeeCreatePageViewModel
            {
                ProjectId = data.ProjectId,
                ManagerId = data.ManagerId,

                ExistingEmployees = data.ExistingEmployees
                    .Select(e => new SelectListItem(e.FullName, e.PersonId.ToString()))
                    .ToList(),

                StepCategories = data.StepCategories
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                    .ToList()
            };

            return View(vm);
        }


        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireBoardRole")]
        // POST: Employee/Create

        public async Task<IActionResult> Create(EmployeeViewModel model, int projectId, string option)
        {
            var cmd = new CreateEmployeeStepCommand
            {
                ProjectId = projectId,
                Option = option,
                StepDescription = model.Employee.Step.Description,
                StartingDate = model.Employee.Step.StartingDate,
                NbreOfDays = model.Employee.Step.NbreOfDays,
                StepBudget = model.Employee.Step.StepBudget,
                StepCategorieId = model.Employee.Step.SelectedStepCategoryId,
                DailySalary = model.Employee.DailySalary,
                ExistingPersonId = model.Employee.SelectedPersonId,
                FirstName = model.Employee.Person?.FirstName,
                LastName = model.Employee.Person?.LastName,
                IdNumber = model.Employee.Person?.IdNumber
            };

            await _apiClient.CreateEmployeasync(cmd);

            return RedirectToAction("Details", "Coops");
        }







        // GET: Employees/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employee.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(employee);
        //}

        //// POST: Employees/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,DailySalary")] Employee employee)
        //{
        //    if (id != employee.EmployeeId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(employee);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeeExists(employee.EmployeeId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(employee);
        //}

        //// GET: Employees/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employee
        //        .FirstOrDefaultAsync(m => m.EmployeeId == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(employee);
        //}

        //// POST: Employees/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var employee = await _context.Employee.FindAsync(id);
        //    _context.Employee.Remove(employee);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employee.Any(e => e.EmployeeId == id);
        //}
    }
}