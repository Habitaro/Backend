using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User : IdentityUser<int>
    {
        public string PasswordSalt { get; set; } = "";

        public int AvatarId { get; set; }

        public string Status { get; set; } = "";

        public int CurrentExp { get; set; }

        public int RequiredExp { get; set; }

        public int RankId { get; set; }

        public Rank Rank { get; set; }

        public IEnumerable<Habit> Habits { get; set; }
    }
}
