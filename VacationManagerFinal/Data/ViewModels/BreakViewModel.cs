using VacationManager.Data.Models;
using System.Web;

namespace VacationManagerFinal.Data.ViewModels
{
    public class BreakViewModel
    {
        public Guid ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime RequestDate { get; set; }
        public bool HalfDay { get; set; }
        public bool Accepted { get; set; }
        public Employee Requestee { get; set; }
        public string BreakType { get; set; }
        public IFormFile Image { get; set; }
    }
}
