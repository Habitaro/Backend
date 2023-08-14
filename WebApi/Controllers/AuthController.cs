using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models.Contracts;
using WebApi.Models.Services;
using WebApi.Models.Services.Abstractions;
using WebApi.Startup.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Authentication")]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork serviceManager, IConfiguration configuration)
        {
            _unit = serviceManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Register(UserCreationDto creationDto)
        {
            await _unit.UserService.Create(creationDto);

            var userModel = await _unit.UserService.GetByEmailAsModel(creationDto.Email);

            var token = JwtHelper.GenerateToken(userModel, _configuration);

            return Ok(token);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Login(UserLoginDto loginDto)
        {

            var user = await _unit.UserService.GetByEmailAsModel(loginDto.Email);

            if (_unit.UserService.VerifyPassword(user, loginDto.Password))
            {
                var token = JwtHelper.GenerateToken(user, _configuration);
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid password");
            }
        }

        
    }
}
