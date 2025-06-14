using Domain.Models;

namespace Domain.Ports.Primary
{
    public interface IDayOffService
    {
        Task<DayOff> GetById(Guid id);

        Task<DayOff> CreateDayOff(DayOff dayOffDetails);
    }
}
