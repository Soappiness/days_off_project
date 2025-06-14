using Domain.Models;
using Domain.Ports.Primary;
using Domain.Ports.Secondary;

namespace Domain.Services
{
    public sealed class DayOffService : IDayOffService
    {
        private readonly IDayOffRepository _dayOffRepository;
        public DayOffService(
            IDayOffRepository dayOffRepository
            ) 
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

            var createdDayOff = await _dayOffRepository.CreateDayOff(dayOffDetails);

            return createdDayOff;
        }
    }
}
