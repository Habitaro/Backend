using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class HabitRepository : IHabitRepository
    {
        private readonly HabitaroDbContext _context;

        public HabitRepository(HabitaroDbContext context)
        {
            _context = context;
        }

        public Task Add(Habit habit)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Habit>> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
