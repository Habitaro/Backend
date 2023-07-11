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

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int AvatarId { get; set; }

        public string Status { get; set; } = null!;

        public Rank Rank { get; set; } = null!;

        public int RankId { get; set; }
    }
}
