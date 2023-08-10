using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    [Serializable]
    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public int AvatarId { get; set; }

        public string Status { get; set; }

        public int CurrentExp { get; set; }

        public int RequiredExp { get; set; }

        public int RankId { get; set; }

        public string Rank { get; set; }
    }
}
