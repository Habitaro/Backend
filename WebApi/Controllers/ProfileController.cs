using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IServiceManager serviceManager;
        private readonly IConfiguration configuration;

        public ProfileController(IServiceManager serviceManager, IConfiguration configuration)
        {
            this.serviceManager = serviceManager;
            this.configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<UserModel> Get()
        {
            var Id = int.Parse(User.FindFirstValue("Id"));
            var userModel = serviceManager.UserService.GetById(Id);

            return Ok(userModel);
        }

        [HttpGet("All")]
        [Authorize]
        public ActionResult<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = serviceManager.UserService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserModel> GetById(int id)
        {
            var authorizedId = User.FindFirstValue("Id");

            if (id.ToString() != authorizedId)
            {
                return Unauthorized();
            }

            var userModel = serviceManager.UserService.GetById(id);
            return Ok(userModel);
        }

        [HttpPatch]
        [Authorize]
        public ActionResult Update(UserForEditDto editDto)
        {
            if (ModelState.IsValid)
            {
                var authorizedId = User.FindFirstValue("Id");
                serviceManager.UserService.Update(editDto, int.Parse(authorizedId));

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public ActionResult Update([FromBody] UserForEditDto dtoModel, int id)
        {
            if (ModelState.IsValid)
            {
                serviceManager.UserService.Update(dtoModel, id);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult Remove()
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            try
            {
                serviceManager.UserService.RemoveById(userId);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"User with Id {userId} was not found");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                serviceManager.UserService.RemoveById(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"User with Id {id} was not found");
            }

            return NoContent();
        }
    }
}
