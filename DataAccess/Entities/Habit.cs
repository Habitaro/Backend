using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Habit
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; } 

        public User User { get; set; }

        public ICollection<HabitDay> Progress { get; set; } = null!;
    }
}
