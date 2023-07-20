using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts
{
    public class UserForEditDto
    {
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z0-9]{2,16}$", ErrorMessage = "Username should contain only letters and digits")]
        public string? Username { get; set; }

        [Range(0, int.MaxValue)]
        public int? AvatarId { get; set; }

        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string? Status { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[/,*\\-+$#@%&])[A-Za-z\\d/,*\\-+$#@%&]{8,20}$",
            ErrorMessage = "Password Should contain at least 1 uppercase, 1 lowercase, 1 digit and 1 special character(/, *, -, +, @, #, $, %, &)")]
        public string? Password { get; set; }
    }
}
