using Domain.Interfaces;
using Domain.Models.Enums;

namespace Domain.Models
{
    public class DayOff : IAggregateRoot
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOffTypeEnum Type { get; set; }

        public DayOffAcceptanceStatusEnum Status { get; set; } = DayOffAcceptanceStatusEnum.Pending;

        public string? StatusReason { get; set; }

        public DateTime? StatusAcceptanceDate { get; set; }

        public Guid EmployeeId { get; set; }

        public string? Comments { get; set; }


        public void Approve()
        {
            if (Status != DayOffAcceptanceStatusEnum.Pending)
                throw new InvalidOperationException("Only pending leave can be approved.");

            Status = DayOffAcceptanceStatusEnum.Approved;
            StatusAcceptanceDate = DateTime.Now;
        }

        public void Refuse()
        {
            if (Status != DayOffAcceptanceStatusEnum.Pending)
                throw new InvalidOperationException("Only pending leave can be refused.");

            Status = DayOffAcceptanceStatusEnum.Refused;
            StatusAcceptanceDate = DateTime.Now;
        }
    }
}
