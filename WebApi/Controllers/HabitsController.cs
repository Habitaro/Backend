using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;
using WebApi.Startup.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Habit tracker")]
    //[ServiceFilter(typeof(GlobalExceptionFilter))]
    public class HabitsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public HabitsController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpPost]
        [SwaggerOperation("Add Habit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddHabit([FromBody]HabitCreationDto dto)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            await _unit.HabitService.Add(dto, userId);
            return NoContent();           
        }

        [HttpGet]
        [SwaggerOperation("Get sorted habits")]
        [ProducesResponseType(typeof(IEnumerable<HabitReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HabitReadDto>>> GetSortedByNameDesc([FromQuery] string? sortingBy)
        {
            int userId = int.Parse(User.FindFirstValue("Id"));
            IEnumerable<HabitReadDto> habits = sortingBy switch
            {
                "Name" => await _unit.HabitService.GetSortedByNameAsc(userId),
                _ => await _unit.HabitService.GetByUserId(userId),
            };
            return Ok(habits);
        }
    }
}
