using AutoMapper;
using Domain.Models.Requests;
using Domain.Models.Response;
using Domain.Ports.Primary;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DayOffController : ControllerBase
    {
        private readonly ILogger<DayOffController> _logger;
        private readonly IMapper _mapper;
        private readonly IDayOffService _dayOffService;

        public DayOffController(
            ILogger<DayOffController> logger,
            IMapper mapper,
            IDayOffService dayOffService)
        {
            _logger = logger;
            _mapper = mapper;
            _dayOffService = dayOffService;
        }

        [HttpPost("create")]
        public async Task<DayOffCreated> CreateDayOffAsync([FromBody]DayOffDto dayOffDetails)
        {
            var dayOff = _mapper.Map<Domain.Models.DayOff>(dayOffDetails);

            var createdDayOff = await _dayOffService.CreateDayOff(dayOff);

            return _mapper.Map<DayOffCreated>(createdDayOff);
        }
    }
}
