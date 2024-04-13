namespace VacationManager.Data.Models
{
    public class Team
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Project? Project { get; set; }
        public List<Employee>? Employees { get; set; }
        public Employee? TeamLead { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
