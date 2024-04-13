using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationManager.Data.Models;

namespace VacationManagerFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Team>()
                .HasMany(e => e.Employees)
                .WithOne(e => e.CurrentTeam)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Project>()
               .HasMany(e => e.Teams)
               .WithOne(e => e.Project)
               .OnDelete(DeleteBehavior.SetNull);
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Break> Breaks { get; set; }
    }
}
