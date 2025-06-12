using Domain.Models;

namespace Domain.Ports.Secondary
{
    public interface IDayOffRepository
    {
        Task<DayOff> GetById(Guid id);

        Task<DayOff> CreateDayOff(DayOff dayOff);
    }
}
