using Domain.Models.Enums;

namespace Application.Models.Response
{
    public class DayOffDto
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOffTypeEnum Type { get; set; }

        public DayOffAcceptanceStatusEnum Status { get; set; }

        public string? StatusReason { get; set; }

        public DateTime? StatusAcceptanceDate { get; set; }

        public Guid EmployeeId { get; set; }

        public string? Comments { get; set; }
    }
}
