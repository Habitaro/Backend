﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Profile data")]
    public class ProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public ProfileController(IUnitOfWork unitOfWord)
        {
            _unit = unitOfWord;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary ="Get all users profile data")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var users = await _unit.UserService.GetAllAsDto();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary ="Get users profile data by Id")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var userModel = await _unit.UserService.GetByIdAsDto(id);

            if (userModel != null)
            {
                return Ok(userModel);
            }

            return NotFound($"User with id {id} was not found");
        }

        [HttpPatch("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Update user`s data by Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromBody] UserEditDto dtoModel, int id)
        {
            if (ModelState.IsValid)
            {
                await _unit.UserService.Update(dtoModel, id);

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Remove user`s profile by Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _unit.UserService.RemoveById(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"User with Id {id} was not found");
            }

            return NoContent();
        }
    }
}
