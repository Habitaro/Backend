namespace WebApi.Models
{
    public class HabitDayModel
    {
        public int HabitId { get; set; }

        public DateOnly Date { get; set; }

        public bool IsCompleted { get; set; }
    }
}
