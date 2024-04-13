using VacationManager.Data.Models;

namespace VacationManagerFinal.Data.ViewModels
{
    public class TeamViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Project { get; set; }
        public string TeamLeadUserName { get; set; }

    }
}
