﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [SwaggerOperation(summary: "Get current user`s habits",
            description: "Options for sorting query: Name, NameDesc, Creation, CreationDesc" +
            "\n\nWithout query collection will be sorted by creation time")]
        [ProducesResponseType(typeof(IEnumerable<HabitReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HabitReadDto>>> GetAll([FromQuery] string? sortBy)
        {
            int userId = int.Parse(User.FindFirstValue("Id"));
            IEnumerable<HabitReadDto> habits = sortBy switch
            {
                "Name" => await _unit.HabitService.GetSortedByNameAsc(userId),
                "NameDesc" => await _unit.HabitService.GetSortedByNameDesc(userId),
                "Creation" => await _unit.HabitService.GetByUserId(userId),
                "CreationDesc" => await _unit.HabitService.GetByUserIdDesc(userId),
                _ => await _unit.HabitService.GetByUserId(userId),
            };
            return Ok(habits);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get habit data by habit id")]
        [ProducesResponseType(typeof(HabitReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<HabitReadDto>> GetById(int id)
        {
            var habit = await _unit.HabitService.GetById(id);
            var userId = int.Parse(User.FindFirstValue("Id"));

            if (habit.UserId == userId)
            {
                return Ok(habit);
            }

            return BadRequest("Access denied");
        }

        [HttpPost]
        [SwaggerOperation("Add Habit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddHabit([FromBody] HabitCreationDto dto)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            await _unit.HabitService.Add(dto, userId);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [SwaggerOperation(summary: "Update habit`s data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] HabitEditDto dto)
        {
            if (dto.Name == null && dto.Description == null)
            {
                return BadRequest("No data to update");
            }

            await _unit.HabitService.Update(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete habit by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            var habit = await _unit.HabitService.GetById(id);
            var userId = int.Parse(User.FindFirstValue("Id"));

            if (habit.UserId != userId)
            {
                throw new InvalidOperationException(message: "Access denied");
            }

            await _unit.HabitService.Delete(id);
            return NoContent();
        }

        [HttpPatch("Progress/{id}")]
        [SwaggerOperation(summary: "Update habit progress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateHabitProgress(int id, ProgressDto dto)
        {
            await _unit.HabitService.UpdateProgress(id, dto);

            return NoContent();
        }
    }
}
