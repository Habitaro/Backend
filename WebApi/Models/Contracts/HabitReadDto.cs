namespace WebApi.Models.Contracts
{
    public class HabitReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Exp { get; set; }

        public ICollection<HabitDayModel> Progress { get; set; }
    }
}
