using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var users = await _unit.UserService.GetAllAsDto();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UserEditDto dtoModel, int id)
        {
            if (await _unit.UserService.GetByIdAsDto(id) != null)
            {
                await _unit.UserService.Update(dtoModel, id);

                return NoContent();
            }

            return NotFound($"User with Id {id} was not found");
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
