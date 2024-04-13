using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationManager.Data.Models;
using VacationManagerFinal.Data;
using VacationManagerFinal.Data.ViewModels;

namespace VacationManagerFinal.Controllers
{
    public class BreakController : Controller
    {
        private ApplicationDbContext context;

        private UserManager<Employee> userManager;
        public BreakController(ApplicationDbContext dbContext, UserManager<Employee> userManager)
        {
            this.context = dbContext;
            this.userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult All()
        {
            return View(context.Breaks.Include(x =>x.Requestee).Where(x=>x.Requestee.UserName == User.Identity.Name).ToList());
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(BreakViewModel addedBreak)
        {
            Break br = new Break();
            Employee req = await context.Employees.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            br.Requestee = req;
            br.DateFrom = addedBreak.DateFrom;
            br.DateTo = addedBreak.DateTo;
            br.RequestDate = DateTime.Now;
            br.BreakType = addedBreak.BreakType;
            br.HalfDay = addedBreak.HalfDay;

            if(addedBreak.Image is not null)
            {
                string filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await addedBreak.Image.CopyToAsync(stream);
                }
                br.Image = System.IO.File.ReadAllBytes(filePath);
            }

            await context.Breaks.AddAsync(br);
            await context.SaveChangesAsync();

            return RedirectToAction("All", "Break");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var currentBreak = await this.context.Breaks.FindAsync(Id);
            if (currentBreak is not null)
            {
                this.context.Breaks.Remove(currentBreak);
                await this.context.SaveChangesAsync();
            }
            return RedirectToAction("All", "Break");
        }
        [HttpPost]
        public async Task<IActionResult> Accept(Guid Id)
        {
            var currentBreak = await this.context.Breaks.FindAsync(Id);
            if (currentBreak is not null)
            {
                currentBreak.Accepted= true;
                await this.context.SaveChangesAsync();
            }
            return RedirectToAction("Manage", "Break");
        }
        [HttpPost]
        public IActionResult Download(Guid id)
        {
            Break br = context.Breaks.Include(x=>x.Requestee).FirstOrDefault(x=>x.ID == id);
            string filePath = br.Requestee.UserName + ".png";
            System.IO.File.WriteAllBytes(filePath, br.Image);
            return File(br.Image,"image/png", filePath);
        }
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            List<Break> breaks = context.Breaks.Include("Requestee.CurrentTeam").Where(x=>!x.Accepted).ToList();
            Employee current = context.Employees.FirstOrDefault(x => x.UserName == User.Identity.Name);
            string role = (await userManager.GetRolesAsync(current)).FirstOrDefault();
            if (role == "Team Lead")
            {
                breaks = breaks.Where(x => x.Requestee.CurrentTeam.TeamLead.UserName == current.UserName && x.Requestee.UserName != User.Identity.Name).ToList();
                foreach(Break currentBreak in breaks)
                {
                    if ((await userManager.GetRolesAsync(currentBreak.Requestee)).First() == "CEO") breaks.Remove(currentBreak);
                }
            }
            return View(breaks);
        }
    }
}
