namespace DataAccess.Entities
{
    public class Rank
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<User> Users { get; set; } = null!;
    }
}