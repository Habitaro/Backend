namespace WebApi.Models.Contracts
{
    public class UserReadDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        public string Rank { get; set; }

        public int RankId { get; set; }

        public int AvatarId { get; set; }

        public int CurrentExp { get; set; }

        public int RequiredExp { get; set; }
    }
}
