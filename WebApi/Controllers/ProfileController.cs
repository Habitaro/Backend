using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IConfiguration _configuration;

        public ProfileController(IUnitOfWork serviceManager, IConfiguration configuration)
        {
            _unit = serviceManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary ="Get current user profile data")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public ActionResult<UserModel> Get()
        {
            var Id = int.Parse(User.FindFirstValue("Id"));
            var userModel = _unit.UserService.GetById(Id);

            return Ok(userModel);
        }

        [HttpGet("All")]
        [Authorize]
        [SwaggerOperation(Summary ="Get all users profile data")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = _unit.UserService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary ="Get users profile data by Id")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public ActionResult<UserModel> GetById(int id)
        {
            var userModel = _unit.UserService.GetById(id);

            if (userModel != null)
            {
                return Ok(userModel);
            }

            return NotFound($"User with id {id} was not found");
        }

        [HttpPatch]
        [Authorize]
        [SwaggerOperation(Summary ="Update current user`s data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Update(UserForEditDto editDto)
        {
            if (ModelState.IsValid)
            {
                var authorizedId = User.FindFirstValue("Id");
                _unit.UserService.Update(editDto, int.Parse(authorizedId));

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPatch("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Update user`s data by Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Update([FromBody] UserForEditDto dtoModel, int id)
        {
            if (ModelState.IsValid)
            {
                _unit.UserService.Update(dtoModel, id);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Authorize]
        [SwaggerOperation(Summary ="Remove current user`s profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Remove()
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            try
            {
                _unit.UserService.RemoveById(userId);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"User with Id {userId} was not found");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Remove user`s profile by Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Delete(int id)
        {
            try
            {
                _unit.UserService.RemoveById(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"User with Id {id} was not found");
            }

            return NoContent();
        }
    }
}
