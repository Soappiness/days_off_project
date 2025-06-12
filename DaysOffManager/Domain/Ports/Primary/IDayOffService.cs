using Domain.Models;

namespace Domain.Ports.Primary
{
    public interface IDayOffService
    {
        Task<DayOff> CreateDayOff(DayOff dayOffDetails);
    }
}
