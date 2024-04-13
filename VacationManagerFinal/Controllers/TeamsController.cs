using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VacationManager.Data.Models;
using VacationManagerFinal.Data;
using VacationManagerFinal.Data.ViewModels;

namespace VacationManagerFinal.Controllers
{
    public class TeamsController : Controller
    {
        public ApplicationDbContext context;
        public TeamsController(ApplicationDbContext dbContext)
        {
            this.context = dbContext;
        }
        [HttpGet]
        public IActionResult All()
        {
            return View(context.Teams.Include(x=>x.TeamLead).Include(x=>x.Project).ToList());
        }
        [HttpGet]
        public IActionResult Add()
        {
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(TeamViewModel model)
        {
            Team team = new Team();
            var teamLeadTemp = context.Employees.ToList().FirstOrDefault(x => x.UserName == model.TeamLeadUserName);
            team.TeamLead = teamLeadTemp;
            team.Name = model.Name;
            var projectTemp = context.Projects.ToList().FirstOrDefault(x => x.Name == model.Project);
            team.Project = projectTemp;

            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();

            return RedirectToAction("All", "Teams");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var team = await this.context.Teams.FindAsync(Id);
            if(team is not null)
            {
                this.context.Teams.Remove(team);
                await this.context.SaveChangesAsync();
            }
            return RedirectToAction("All", "Teams");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Team team = context.Teams.Include(x => x.TeamLead).Include(x => x.Project).ToList().FirstOrDefault(x => x.ID == id);
            if (team is not null)
            {
                var teamModel = new TeamViewModel();
                teamModel.ID = id;
                teamModel.Name = team.Name;
                teamModel.TeamLeadUserName = team.TeamLead.ToString();
                teamModel.Project = team.Project.ToString();

                return View(teamModel);
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Edit(TeamViewModel model)
        {
            Team emp = new Team();
            emp = await context.Teams.FindAsync(model.ID);
            string oldName = emp.Name;
            if (emp is not null)
            {
                emp.TeamLead = context.Employees.ToList().FirstOrDefault(x => x.UserName == model.TeamLeadUserName);
                emp.Name = model.Name;
                emp.Project = context.Projects.ToList().FirstOrDefault(x => x.Name == model.Project);

                foreach (var item in context.Employees.Where(x => x.CurrentTeam.Name == oldName))
                {
                    item.CurrentTeam = emp;
                }
            }
          
            await context.SaveChangesAsync();
       
            return RedirectToAction("All", "Teams");
        }
    }
}
