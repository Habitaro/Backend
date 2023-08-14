using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IHabitRepository
    {
        Task Add(Habit habit);

        Task<ICollection<Habit>> GetByUserId(int userId);
    }
}
