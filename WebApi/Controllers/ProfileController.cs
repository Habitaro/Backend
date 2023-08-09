using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;
using WebApi.Startup.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Profile data")]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
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
            return Ok(userModel);
        }

        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UserEditDto dtoModel, int id)
        {
            await _unit.UserService.Update(dtoModel, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _unit.UserService.RemoveById(id);
            return NoContent();
        }
    }
}
