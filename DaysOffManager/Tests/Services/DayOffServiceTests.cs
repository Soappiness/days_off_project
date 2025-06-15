using Domain.Models;
using Domain.Models.Enums;
using Domain.Ports.Secondary;
using Domain.Services;
using Moq;

namespace Tests.Services
{
    public class DayOffServiceTests
    {

        #region Get DayOff By Id Tests

        [Fact]
        public async Task Get_DayOff_By_Id_Returns_DayOff()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffId = Guid.NewGuid();

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            var expectedDayOff = new DayOff
            {
                Id = dayOffId,
            };

            mockDayOffRepository.Setup(repo => repo.GetById(dayOffId))
                .ReturnsAsync(expectedDayOff);

            var returnedDayOff = await dayOffService.GetById(dayOffId);

            Assert.NotNull(returnedDayOff);
            Assert.Equal(dayOffId, returnedDayOff.Id);
        }

        [Fact]
        public async Task Get_DayOff_By_Id_Returns_KeyNotFoundException_When_DayOff_Not_Found()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffId = Guid.NewGuid();

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            mockDayOffRepository
                .Setup(repo => repo.GetById(dayOffId))
                .ReturnsAsync((DayOff)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await dayOffService.GetById(dayOffId));
        }

        #endregion

        #region Create DayOff Tests

        [Fact]
        public async Task Create_DayOff_Returns_DayOff()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var employeeId = Guid.NewGuid();

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            var dayOff = new DayOff
            {
                Id = Guid.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Dentist appointment",
                StatusReason = null,
                StatusAcceptanceDate = null
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
                StatusReason = null,
                StatusAcceptanceDate = null
            };

            mockDayOffRepository
                .Setup(repo => repo.CreateDayOff(dayOff))
                .ReturnsAsync(expectedReturnedDayOff);

            var returnedDayOff = await dayOffService.CreateDayOff(dayOff);

            Assert.NotNull(returnedDayOff);
            Assert.Equal(returnedDayOff, expectedReturnedDayOff);
        }

        [Fact]
        public async Task Create_DayOff_Returns_ArgumentException_When_StartDate_Is_After_EndDate()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            var dayOff = new DayOff
            {
                Id = Guid.Empty,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now,
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = Guid.NewGuid(),
                Comments = "Dentist appointment",
                StatusReason = null
            };

            mockDayOffRepository
                .Setup(repo => repo.CreateDayOff(dayOff))
                .ThrowsAsync(new ArgumentException("Start date must be before end date while submitting a new day off."));

            await Assert.ThrowsAsync<ArgumentException>(async () => await dayOffService.CreateDayOff(dayOff));
        }

        #endregion

        #region Approve DayOff Tests

        [Fact]
        public async Task Approve_DayOff_By_Id_With_Reason_Returns_Approved_DayOff()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var statusReason = "Approved for vacation";

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            var expectedDayOff = new DayOff
            {
                Id = dayOffId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Vacation request",
                StatusReason = null,
                StatusAcceptanceDate = null
            };

            mockDayOffRepository
                .Setup(repo => repo.GetById(dayOffId))
                .ReturnsAsync(expectedDayOff);

            mockDayOffRepository
                .Setup(repo => repo.UpdateDayOff(It.IsAny<DayOff>()))
                .ReturnsAsync((DayOff dayOff) => dayOff);

            var returnedDayOff = await dayOffService.ApproveDayOff(dayOffId, statusReason);

            Assert.Equal(DayOffAcceptanceStatusEnum.Approved, returnedDayOff.Status);
            Assert.Equal(statusReason, returnedDayOff.StatusReason);
            Assert.NotNull(returnedDayOff.StatusAcceptanceDate);
            Assert.IsType<DateTime>(returnedDayOff.StatusAcceptanceDate);
        }

        [Fact]
        public async Task Approve_DayOff_By_Id_With_Reason_Returns_KeyNotFoundException_When_DayOff_Not_Found()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffId = Guid.NewGuid();
            var statusReason = "Approved for vacation";

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            mockDayOffRepository
                .Setup(repo => repo.GetById(dayOffId))
                .ReturnsAsync((DayOff)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await dayOffService.ApproveDayOff(dayOffId, statusReason));
        }

        #endregion

        #region Refuse DayOff Tests

        [Fact]
        public async Task Refuse_DayOff_By_Id_With_Reason_Returns_Refused_DayOff()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var statusReason = "Refused for insufficient amout of RTT remaining.";

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            var expectedDayOff = new DayOff
            {
                Id = dayOffId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DayOffTypeEnum.RTT,
                Status = DayOffAcceptanceStatusEnum.Pending,
                EmployeeId = employeeId,
                Comments = "Vacation request",
                StatusReason = null,
                StatusAcceptanceDate = null
            };

            mockDayOffRepository
                .Setup(repo => repo.GetById(dayOffId))
                .ReturnsAsync(expectedDayOff);

            mockDayOffRepository
                .Setup(repo => repo.UpdateDayOff(It.IsAny<DayOff>()))
                .ReturnsAsync((DayOff dayOff) => dayOff);

            var returnedDayOff = await dayOffService.RefuseDayOff(dayOffId, statusReason);

            Assert.Equal(DayOffAcceptanceStatusEnum.Refused, returnedDayOff.Status);
            Assert.Equal(statusReason, returnedDayOff.StatusReason);
            Assert.NotNull(returnedDayOff.StatusAcceptanceDate);
            Assert.IsType<DateTime>(returnedDayOff.StatusAcceptanceDate);
        }

        [Fact]
        public async Task ARefuse_DayOff_By_Id_With_Reason_Returns_KeyNotFoundException_When_DayOff_Not_Found()
        {
            var mockDayOffRepository = new Mock<IDayOffRepository>();

            var dayOffId = Guid.NewGuid();
            var statusReason = "Approved for vacation";

            var dayOffService = new DayOffService(mockDayOffRepository.Object);

            mockDayOffRepository
                .Setup(repo => repo.GetById(dayOffId))
                .ReturnsAsync((DayOff)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await dayOffService.RefuseDayOff(dayOffId, statusReason));
        }

        #endregion
    }
}
