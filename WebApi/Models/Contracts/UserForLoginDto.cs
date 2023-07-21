﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts
{
    public class UserForLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}