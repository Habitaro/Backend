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
        
        public void Add(User user) 
        {
            _context.Users.Add(user);
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.Include(u => u.Rank).ToList();
            return users;
        }

        public User? GetById(int id)
        {
            var user = _context.Users.Include(u => u.Rank).SingleOrDefault(u => u.Id == id);
            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public User? GetByEmail(string email)
        {
            var user = _context.Users.Include(u => u.Rank).SingleOrDefault(u => u.Email == email);
            return user;
        }
    }
}
