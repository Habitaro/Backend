﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [AllowAnonymous]
        public IActionResult Register(UserForCreationDto model)
        {
            if (ModelState.IsValid) 
            {
                if (serviceManager.UserService.GetByEmail(model.Email) != null)
                {
                    return BadRequest(error: "Email is already registered");
                }

                serviceManager.UserService.Create(model);

                var userModel = serviceManager.UserService.GetByEmail(model.Email)!;

                var issuer = configuration["Jwt:Issuer"];
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", userModel.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, userModel.Username),
                        new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
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

                return Ok(stringToken);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserModel model)
        {
            var user = serviceManager.UserService.GetByEmail(model.Email);
            if (user.Password == model.Password)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
