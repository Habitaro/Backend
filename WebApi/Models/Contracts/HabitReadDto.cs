namespace WebApi.Models.Contracts
{
    public class HabitReadDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public IDictionary<string, bool> Progress { get; set; }
    }
}
