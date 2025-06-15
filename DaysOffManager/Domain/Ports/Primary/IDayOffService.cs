using Domain.Models;

namespace Domain.Ports.Primary
{
    public interface IDayOffService
    {
        Task<DayOff> GetById(Guid id);

        Task<DayOff> CreateDayOff(DayOff dayOffDetails);

        Task<DayOff> ApproveDayOff(Guid id, string statusReason);

        Task<DayOff> RefuseDayOff(Guid id, string statusReason);
    }
}
