using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts
{
    public class HabitCreationDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Habit {0} length should be between {2} and {1}", MinimumLength = 1)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
