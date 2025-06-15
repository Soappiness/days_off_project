using Application.Models.Requests;
using Application.Models.Response;
using Asp.Versioning;
using AutoMapper;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Ports.Primary;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DayOffController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDayOffService _dayOffService;
        private readonly IValidator<DayOffCreateDto> _dayOffDtoValidator;
        private readonly IValidator<StatusReasonRequestDto> _statusReasonRequestDtoValidator;

        public DayOffController(
            IMapper mapper,
            IDayOffService dayOffService,
            IValidator<DayOffCreateDto> dayOffDtoValidator,
            IValidator<StatusReasonRequestDto> statusReasonRequestDtoValidator)
        {
            _mapper = mapper;
            _dayOffService = dayOffService;
            _dayOffDtoValidator = dayOffDtoValidator;
            _statusReasonRequestDtoValidator = statusReasonRequestDtoValidator;
        }

        /// <summary>
        /// Get a day off by its ID.
        /// </summary>
        /// <param name="id">Day off id</param>
        /// <returns>The day off matching the id.</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dayOff = await _dayOffService.GetById(id);

            return Ok(_mapper.Map<DayOffDto>(dayOff));
        }

        /// <summary>
        /// Create a new day off.
        /// </summary>
        /// <param name="dayOffDetails">DTO that represent a day off for creation</param>
        /// <returns>The created day off by the route GetById</returns>
        /// <exception cref="ValidationException">Exception thrown if the DTO's properties are not valid.</exception>
        [HttpPost]
        public async Task<IActionResult> CreateDayOffAsync([FromBody] DayOffCreateDto dayOffDetails)
        {
            var validationResult = await _dayOffDtoValidator.ValidateAsync(dayOffDetails);
            if (!validationResult.IsValid)
                throw new ValidationException("One or many properties are invalid.", validationResult.Errors);

            var dayOff = _mapper.Map<DayOff>(dayOffDetails);

            var createdDayOff = await _dayOffService.CreateDayOff(dayOff);

            return CreatedAtAction(nameof(GetById), new { id = createdDayOff.Id }, _mapper.Map<DayOffDto>(createdDayOff));
        }

        /// <summary>
        /// Update the status of a day off to "Approved" with reasons.
        /// </summary>
        /// <param name="id">The day off's id</param>
        /// <param name="statusReasonRequestDto">The reason used to approve the day off.</param>
        /// <returns>The updated day off.</returns>
        /// <exception cref="ValidationException">Exception thrown if the reason's property is not valid.</exception>
        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> ApproveDayOffAsync(Guid id, [FromBody] StatusReasonRequestDto statusReasonRequestDto)
        {
            var validationResult = await _statusReasonRequestDtoValidator.ValidateAsync(statusReasonRequestDto);
            if (!validationResult.IsValid)
                throw new ValidationException("Status reason is invalid.", validationResult.Errors);

            var approvedDayOff = await _dayOffService.ApproveDayOff(id, statusReasonRequestDto.StatusReason);

            return Ok(_mapper.Map<DayOffDto>(approvedDayOff));
        }

        /// <summary>
        /// Update the status of a day off to "Refused" with reasons.
        /// </summary>
        /// <param name="id">The day off's id</param>
        /// <param name="statusReasonRequestDto">The reason used to refuse the day off.</param>
        /// <returns>The updated day off.</returns>
        /// <exception cref="ValidationException">Exception thrown if the reason's property is not valid.</exception>
        [HttpPatch("{id}/refuse")]
        public async Task<IActionResult> RefuseDayOffAsync(Guid id, [FromBody] StatusReasonRequestDto statusReasonRequestDto)
        {
            var validationResult = await _statusReasonRequestDtoValidator.ValidateAsync(statusReasonRequestDto);
            if (!validationResult.IsValid)
                throw new ValidationException("Status reason is invalid.", validationResult.Errors);

            var refusedDayOff = await _dayOffService.RefuseDayOff(id, statusReasonRequestDto.StatusReason);

            return Ok(_mapper.Map<DayOffDto>(refusedDayOff));
        }
    }
}
