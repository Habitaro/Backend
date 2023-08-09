namespace DataAccess.Entities
{
    public class Rank
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public ICollection<User> Users { get; set; } = null!;
    }
}