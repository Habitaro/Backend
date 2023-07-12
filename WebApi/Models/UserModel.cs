using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    [Serializable]
    public class UserModel
    {
        [Required]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int AvatarId { get; set; }

        public string Status { get; set; }

        public int RankId { get; set; }
    }
}
