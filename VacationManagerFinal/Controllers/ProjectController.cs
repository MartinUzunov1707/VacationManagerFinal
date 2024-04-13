using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationManager.Data.Models;
using VacationManagerFinal.Data;
using VacationManagerFinal.Data.ViewModels;

namespace VacationManagerFinal.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext context;
        public ProjectController(ApplicationDbContext dbContext)
        {
            this.context = dbContext;
        }
        public IActionResult All()
        {
            return View(context.Projects.ToList());
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Project model)
        {
            await context.Projects.AddAsync(model);
            await context.SaveChangesAsync();
            
            return RedirectToAction("All","Project");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var team = await this.context.Projects.FindAsync(Id);
            if (team is not null)
            {
                this.context.Projects.Remove(team);
                await this.context.SaveChangesAsync();
            }
            return RedirectToAction("All", "Project");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Project proj = await this.context.Projects.FindAsync(id);
            if (proj is not null)
            {
                var projectModel = new Project();
                projectModel.ID = id;
                projectModel.Name = proj.Name;
                projectModel.Description = proj.Description;

                return View(projectModel);
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Project model)
        {
            Project emp = new Project();
            emp = await context.Projects.FindAsync(model.ID);
            string oldName = emp.Name;
            if (emp is not null)
            {
                emp.Name = model.Name;
                emp.Description = model.Description;
                foreach(var elem in context.Teams.Include(x=>x.Project).Where(x=>x.Project.Name == oldName)){
                    elem.Project = emp;
                }
            }

            await context.SaveChangesAsync();

            return RedirectToAction("All", "Project");
        }
    }
}
