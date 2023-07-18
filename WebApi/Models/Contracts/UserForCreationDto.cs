using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts
{
    public class UserForCreationDto
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]{2,16}$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[/,*\\-+$#@%&])[A-Za-z\\d/,*\\-+$#@%&]{8,}$")]
        public string Password { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[/,*\\-+$#@%&])[A-Za-z\\d/,*\\-+$#@%&]{8,}$")]
        [Compare("Password")]
        public string Password_Confirm { get; set; }
    }
}
