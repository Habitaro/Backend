using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts
{
    public class ProgressDto
    {
        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public bool Status { get; set; } 
    }
}
