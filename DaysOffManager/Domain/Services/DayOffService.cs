using Domain.Models;
using Domain.Ports.Primary;
using Domain.Ports.Secondary;

namespace Domain.Services
{
    public sealed class DayOffService : IDayOffService
    {
        private readonly IDayOffRepository _dayOffRepository;

        public DayOffService(IDayOffRepository dayOffRepository)
        {
            _dayOffRepository = dayOffRepository;
        }

        public async Task<DayOff> GetById(Guid id)
        {
            var dayOff = await _dayOffRepository.GetById(id);

            if (dayOff == null)
            {
                throw new KeyNotFoundException($"Day off with ID {id} not found.");
            }

            return dayOff;
        }

        public async Task<DayOff> CreateDayOff(DayOff dayOffDetails)
        {
            if (dayOffDetails.StartDate >= dayOffDetails.EndDate)
            {
                throw new ArgumentException("Start date must be before end date while submitting a new day off.");
            }

            return await _dayOffRepository.CreateDayOff(dayOffDetails);
        }

        public async Task<DayOff> ApproveDayOff(Guid id, string statusReason)
        {
            var dayOff = await GetById(id);

            if (dayOff == null)
            {
                throw new KeyNotFoundException($"Day off with ID {id} not found.");
            }

            dayOff.Approve();
            dayOff.StatusReason = statusReason ?? null;

            return await _dayOffRepository.UpdateDayOff(dayOff);
        }

        public async Task<DayOff> RefuseDayOff(Guid id, string statusReason)
        {
            var dayOff = await GetById(id);

            if (dayOff == null)
            {
                throw new KeyNotFoundException($"Day off with ID {id} not found.");
            }

            dayOff.Refuse();
            dayOff.StatusReason = statusReason ?? null;

            return await _dayOffRepository.UpdateDayOff(dayOff);
        }
    }
}
