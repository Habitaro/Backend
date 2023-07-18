﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Register(UserForCreationDto model)
        {
            if (ModelState.IsValid) 
            {
                if (serviceManager.UserService.GetByEmail(model.Email) != null)
                {
                    return BadRequest(error: "Email is already registered");
                }

                serviceManager.UserService.Create(model);
                return Ok();
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
