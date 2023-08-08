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
    [SwaggerTag("Profile data")]
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

        [HttpPatch("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Update user`s data by Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Update([FromBody] UserForEditDto dtoModel, int id)
        {
            if (ModelState.IsValid)
            {
                _unit.UserService.Update(dtoModel, id);

                return NoContent();
            }

            return BadRequest(ModelState);
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
