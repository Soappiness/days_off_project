using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    public class DayOffCreated
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOffTypeEnum Type { get; set; }

        public DayOffAcceptanceStatusEnum Status { get; set; }

        public string? StatusReason { get; set; }

        public Guid EmployeeId { get; set; }

        public string? Comments { get; set; }
    }
}
