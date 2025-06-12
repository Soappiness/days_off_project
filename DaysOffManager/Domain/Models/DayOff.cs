using Domain.Models.Enums;

namespace Domain.Models
{
    public class DayOff
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOffTypeEnum Type { get; set; }

        public DayOffAcceptanceStatusEnum Status { get; set; } = DayOffAcceptanceStatusEnum.Pending;

        public string? StatusReason { get; set; }

        public Guid EmployeeId { get; set; }

        public string? Comments { get; set; }
    }
}
