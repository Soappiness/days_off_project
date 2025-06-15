using Domain.Models;
using Domain.Ports.Secondary;

namespace Infrastructure.Adapters
{
    public class DayOffRepository : IDayOffRepository
    {
        private readonly AppDbContext _context;

        public DayOffRepository(AppDbContext context) => _context = context;

        public async Task<DayOff> GetById(Guid id)
        {
            return await _context.DaysOffs.FindAsync(id);
        }

        public async Task<DayOff> CreateDayOff(DayOff dayOff)
        {
            await _context.DaysOffs.AddAsync(dayOff);
            await _context.SaveChangesAsync();

            return dayOff;
        }

        public async Task<DayOff> UpdateDayOff(DayOff dayOff)
        {
            _context.DaysOffs.Update(dayOff);
            await _context.SaveChangesAsync();

            return dayOff;
        }
    }
}
