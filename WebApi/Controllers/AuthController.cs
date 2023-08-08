﻿using Microsoft.AspNetCore.Authorization;
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
    [SwaggerTag("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork serviceManager, IConfiguration configuration)
        {
            this._unit = serviceManager;
            this._configuration = configuration;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
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
                if (_unit.UserService.GetByEmail(creationDto.Email) != null)
                {
                    return BadRequest(error: "Email is already registered");
                }

                _unit.UserService.Create(creationDto, _configuration["PasswordPepper"]);

                var userModel = _unit.UserService.GetByEmail(creationDto.Email)!;

                var token = GenerateToken(userModel);

                return Ok(token);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary ="Log in", Description ="Require valid email and password. Returns JWT")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Login(UserForLoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = _unit.UserService.GetByEmail(loginDto.Email);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                if (_unit.UserService.VerifyPassword(user, loginDto.Password, _configuration["PasswordPepper"]))
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
            var issuer = _configuration["Jwt:Issuer"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("Id", model.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, model.Username),
                        new Claim(JwtRegisteredClaimNames.Email, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                Expires = DateTime.Now.AddMinutes(20),
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
