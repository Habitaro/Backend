﻿using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly HabitaroDbContext context;

        public IUserRepository UserRepository { get; }

        public IHabitRepository HabitRepository { get; }

        public RepositoryManager(HabitaroDbContext context)
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            HabitRepository = new HabitRepository(context);
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
