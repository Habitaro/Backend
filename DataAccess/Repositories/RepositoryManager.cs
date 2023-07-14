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
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
