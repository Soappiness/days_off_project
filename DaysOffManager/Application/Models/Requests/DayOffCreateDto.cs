using Domain.Models.Enums;

namespace Domain.Models.Requests
{
    public class DayOffCreateDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOffTypeEnum Type { get; set; }

        public Guid EmployeeId { get; set; }

        public string? Comments { get; set; }
    }
}
