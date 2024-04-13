using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VacationManager.Data.Models
{
    public class Employee : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Team? CurrentTeam { get; set; }
        public override string ToString()
        {
            return this.UserName;   
        }
    }
}
