namespace VacationManager.Data.Models
{
    public class Break
    {
        public Guid ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime RequestDate { get; set; }
        public bool HalfDay { get; set; }
        public bool Accepted { get; set; }
        public Employee Requestee { get; set; }
        public string BreakType { get; set; }
        public byte[]? Image { get; set; }
    }
}
