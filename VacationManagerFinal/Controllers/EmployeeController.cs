using Microsoft.AspNetCore.Mvc;
using VacationManagerFinal.Data;
using VacationManager.Data.Models;
using VacationManagerFinal.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VacationManagerFinal.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext context;
        private UserManager<Employee> UserManager;
        public EmployeeController(ApplicationDbContext dbContext, UserManager<Employee> userManager)
        {
            this.context = dbContext;     
            this.UserManager = userManager;
        }
        [HttpGet]
        public IActionResult All()
        {
            return View(context.Employees.Include(x=>x.CurrentTeam).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Employee employee = context.Employees.Include(x=>x.CurrentTeam).ToList().FirstOrDefault(x => x.Id == id.ToString());
            if(employee is not null)
            {
                var employeeModel = new EmployeeViewModel();
                employeeModel.Id = id;
                employeeModel.FirstName = employee.FirstName;
                employeeModel.LastName = employee.LastName;
                employeeModel.UserName = employee.UserName;
                if(employee.CurrentTeam is not null) employeeModel.TeamName = employee.CurrentTeam.ToString();
                employeeModel.Role = (await UserManager.GetRolesAsync(employee)).FirstOrDefault();
                return View(employeeModel);
            }
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            Employee emp = await context.Employees.FindAsync(model.Id.ToString());
            if(emp is not null)
            {
                emp.FirstName = model.FirstName;
                emp.LastName = model.LastName;
                emp.UserName = model.UserName;
                emp.CurrentTeam = context.Teams.ToList().FirstOrDefault(x=>x.Name == model.TeamName);
                await UserManager.RemoveFromRoleAsync(emp, (await UserManager.GetRolesAsync(emp)).FirstOrDefault());
                await UserManager.AddToRoleAsync(emp, model.Role);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("All","Employee");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var team = await this.context.Employees.FindAsync(Id.ToString());
            if (team is not null)
            {
                this.context.Employees.Remove(team);
                await this.context.SaveChangesAsync();
            }
            return RedirectToAction("All", "Employee");
        }

    }
}
