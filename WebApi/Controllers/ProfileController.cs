using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Models;
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
    }
}
