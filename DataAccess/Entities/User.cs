using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public int AvatarId { get; set; }

        public string Status { get; set; } = string.Empty;

        public int CurrentExp { get; set; }

        public int RequiredExp { get; set; }

        public Rank Rank { get; set; }

        public int RankId { get; set; }
    }
}
