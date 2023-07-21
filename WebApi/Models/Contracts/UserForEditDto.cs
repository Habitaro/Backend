using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Models.Contracts
{
    public class UserForEditDto
    {
        [AllowNull]
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z0-9]{2,16}$", ErrorMessage = "Username should contain only letters and digits")]
        public string? Username { get; set; }

        [AllowNull]
        [Range(0, 20)]
        public int? AvatarId { get; set; }

        [AllowNull]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string? Status { get; set; }
    }
}
