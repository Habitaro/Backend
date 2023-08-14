using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts
{
    public class HabitCreationDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
