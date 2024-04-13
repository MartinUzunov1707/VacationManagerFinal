using Microsoft.Identity.Client;

namespace VacationManager.Data.Models
{
    public class Project
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Team>? Teams { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
