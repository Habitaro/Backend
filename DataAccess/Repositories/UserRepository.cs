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
        private readonly HabitaroDbContext context;

        public UserRepository(HabitaroDbContext context)
        {
            this.context = context;
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
        }
        
        public void Add(User user) 
        {
            context.Users.Add(user);
        }

        public IEnumerable<User> GetAll()
        {
            var users = context.Users.Include(u => u.Rank).ToList();
            return users;
        }

        public User? GetById(int id)
        {
            var user = context.Users.Include(u => u.Rank).SingleOrDefault(u => u.Id == id);
            return user;
        }

        public void Update(User user)
        {
            context.Users.Update(user);
        }

        public User? GetByEmail(string email)
        {
            var user = context.Users.Include(u => u.Rank).SingleOrDefault(u => u.Email == email);
            return user;
        }
    }
}
