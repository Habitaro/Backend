using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
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

        public void DeleteById(int id)
        {
            var user = context.Find<User>(id);
            context.Users.Remove(user);
        }

        public IEnumerable<User> GetAll()
        {
            var users = context.Users.ToList();
            return users;
        }

        public User GetById(int id)
        {
            var user = context.Find<User>(id);
            return user;
        }

        public void Update(User user)
        {
            context.Users.Update(user);
        }
    }
}
