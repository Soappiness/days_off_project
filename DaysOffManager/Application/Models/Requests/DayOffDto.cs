using Domain.Models.Enums;

namespace Domain.Models.Requests
{
    public class DayOffDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOffTypeEnum Type { get; set; }

        public Guid EmployeeId { get; set; }

        public string? Comments { get; set; }
    }
}
