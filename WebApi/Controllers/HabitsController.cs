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
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    public class HabitsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public HabitsController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpPost]
        public async Task<IActionResult> AddHabit(HabitCreationDto dto)
        {
            await _unit.HabitService.Add(dto);
            var userId = int.Parse(User.FindFirstValue("Id"));
            var habits = await _unit.HabitService.GetByUserId(userId);
            return CreatedAtAction("GetByUserId", habits.Last());
        }
    }
}
