using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
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

        public async Task Add(Habit habit)
        {
            await _context.AddAsync(habit);
        }

        public async Task<Habit?> GetById(int id)
        {
            var habit = await _context.Habits.AsNoTracking().Include(h => h.Progress).SingleOrDefaultAsync(h => h.Id == id);
            return habit;
        }

        public async Task<IEnumerable<Habit>> GetByUserId(int userId)
        {
            var Habits = await _context.Habits.Include(h => h.Progress).Where(h => h.UserId == userId).ToListAsync();
            return Habits;
        }

        public void Delete(Habit habit)
        {
            _context.Habits.Remove(habit);
        }
    }
}
