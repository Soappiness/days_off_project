using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Ports.Primary;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DayOffController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDayOffService _dayOffService;
        private readonly IValidator<DayOffCreateDto> _dayOffDtoValidator;

        public DayOffController(
            IMapper mapper,
            IDayOffService dayOffService,
            IValidator<DayOffCreateDto> validator)
        {
            _mapper = mapper;
            _dayOffService = dayOffService;
            _dayOffDtoValidator = validator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dayOff = await _dayOffService.GetById(id);

            return Ok(_mapper.Map<DayOffDto>(dayOff));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDayOffAsync([FromBody]DayOffCreateDto dayOffDetails)
        {
            var validationResult = await _dayOffDtoValidator.ValidateAsync(dayOffDetails);
            if (!validationResult.IsValid)
                throw new ValidationException("One or many properties are invalid.", validationResult.Errors);

            var dayOff = _mapper.Map<DayOff>(dayOffDetails);

            var createdDayOff = await _dayOffService.CreateDayOff(dayOff);

            return CreatedAtAction(nameof(GetById), new { id = createdDayOff.Id }, _mapper.Map<DayOffDto>(createdDayOff));
        }
    }
}
