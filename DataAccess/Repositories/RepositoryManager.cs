using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class RepositoryManager : IRepositoryManager
    {
        private readonly HabitaroDbContext context;

        public IUserRepository UserRepository { get; }

        public RepositoryManager(HabitaroDbContext context)
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            SeedData();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private void SeedData()
        {
            if (!context.Users.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    context.Ranks.Add(new Rank() { Id = i, Name = $"Silver cat {i}" });
                    context.Users.Add(new User()
                    {
                        Id = i,
                        Username = $"User{i}",
                        Email = $"user{i}@gmail.com",
                        AvatarId = i,
                        Password = $"userPassword{i}",
                        RankId = i,,
                        Status = "Sleepin",
                    });
                }
            }
        }
    }
}
