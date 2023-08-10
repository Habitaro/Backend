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
    internal class UserRepository : IUserRepository
    {
        private readonly HabitaroDbContext _context;

        public UserRepository(HabitaroDbContext context)
        {
            _context = context;
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
        
        public async Task Add(User user) 
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.AsNoTracking().Include(u => u.Rank).ToListAsync();
            return users;
        }

        public async Task<User?> GetById(int id)
        {
            var user = await _context.Users.Include(u => u.Rank).SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User?> GetByIdAsNoTracking(int id)
        {
            var user = await _context.Users.AsNoTracking().Include(u => u.Rank).SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = await _context.Users.AsNoTracking().Include(u => u.Rank).SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
