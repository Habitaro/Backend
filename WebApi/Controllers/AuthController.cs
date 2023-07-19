using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [SwaggerTag("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager serviceManager;
        private readonly IConfiguration configuration;

        public AuthController(IServiceManager serviceManager, IConfiguration configuration)
        {
            this.serviceManager = serviceManager;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        [SwaggerOperation(Summary ="Registration", 
            Description ="Require valid user info: username(length = 2..16, allowed letters and digits)," +
            " valid email, password(length = 8..20, at least one lower-, uppercase, digit and special " +
            @"symbol{/,*,-,+, $, #, @, %, &}). Returns JWT")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Register(UserForCreationDto creationDto)
        {
            if (ModelState.IsValid) 
            {
                if (serviceManager.UserService.GetByEmail(creationDto.Email) != null)
                {
                    return BadRequest(error: "Email is already registered");
                }

                serviceManager.UserService.Create(creationDto);

                var userModel = serviceManager.UserService.GetByEmail(creationDto.Email)!;

                var token = GenerateToken(userModel);

                return Ok(token);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        [SwaggerOperation(Summary ="Log in", Description ="Require valid email and password. Returns JWT")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Login(UserForLoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = serviceManager.UserService.GetByEmail(loginDto.Email);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                if (serviceManager.UserService.VerifyPassword(user, loginDto.Password))
                {
                    var token = GenerateToken(user);
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Invalid password");
                }
            }

            return BadRequest(ModelState);
        }

        private string GenerateToken(UserModel model)
        {
            var issuer = configuration["Jwt:Issuer"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("Id", model.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, model.Username),
                        new Claim(JwtRegisteredClaimNames.Email, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                Expires = DateTime.Now.AddMinutes(5),
                Issuer = issuer,
                Audience = issuer,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
