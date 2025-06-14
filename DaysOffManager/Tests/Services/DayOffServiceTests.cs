using Domain.Models;
using Domain.Models.Enums;
using Domain.Ports.Primary;
using Moq;

namespace Tests.Services
{
    public class DayOffServiceTests
    {
        [Fact]
        public async Task Get_DayOff_By_Id_Returns_DayOff()
        {
            var mockDayOffService = new Mock<IDayOffService>();

            var dayOffId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var expectedDayOff = new DayOff
            {
                Id = dayOffId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Dentist appointment",
                StatusReason = null
            };

            mockDayOffService
                .Setup(s => s.GetById(dayOffId))
                .ReturnsAsync(expectedDayOff);

            var returnedDayOff = await mockDayOffService.Object.GetById(dayOffId);

            Assert.NotNull(returnedDayOff);
            Assert.Equal(expectedDayOff, returnedDayOff);
        }

        [Fact]
        public async Task Get_DayOff_By_Id_Returns_KeyNotFoundException_When_DayOff_Not_Found()
        {
            var mockDayOffService = new Mock<IDayOffService>();

            var dayOffId = Guid.NewGuid();

            mockDayOffService
                .Setup(s => s.GetById(dayOffId))
                .ThrowsAsync(new KeyNotFoundException($"Day off with ID {dayOffId} not found."));


            await Assert.ThrowsAsync<KeyNotFoundException>(() => mockDayOffService.Object.GetById(dayOffId));
        }

        [Fact]
        public async Task Create_DayOff_Returns_DayOff()
        {
            var mockDayOffService = new Mock<IDayOffService>();
            var employeeId = Guid.NewGuid();

            var dayOff = new DayOff
            {
                Id = Guid.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Dentist appointment",
                StatusReason = null
            };

            var expectedReturnedDayOff = new DayOff
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Dentist appointment",
                StatusReason = null
            };

            mockDayOffService
                .Setup(s => s.CreateDayOff(It.IsAny<DayOff>()))
                .ReturnsAsync(expectedReturnedDayOff);

            var returnedDayOff = await mockDayOffService.Object.CreateDayOff(dayOff);

            Assert.NotNull(returnedDayOff);
            Assert.Equal(returnedDayOff, expectedReturnedDayOff);
        }

        [Fact]
        public async Task Create_DayOff_Returns_ArgumentException_When_StartDate_Is_After_EndDate()
        {
            var mockDayOffService = new Mock<IDayOffService>();

            var employeeId = Guid.NewGuid();

            var dayOff = new DayOff
            {
                Id = Guid.Empty,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now,
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Dentist appointment",
                StatusReason = null
            };

            mockDayOffService
                .Setup(s => s.CreateDayOff(It.IsAny<DayOff>()))
                .ThrowsAsync(new ArgumentException("Start date must be before end date while submitting a new day off."));

            await Assert.ThrowsAsync<ArgumentException>(() => mockDayOffService.Object.CreateDayOff(dayOff));
        }
    }
}
