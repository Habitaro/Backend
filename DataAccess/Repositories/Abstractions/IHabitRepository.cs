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
        void Delete(Habit habit);
        Task<IEnumerable<Habit>> GetAll();
        Task<Habit?> GetById(int id);
        Task<IEnumerable<Habit>> GetByUserId(int userId);
    }
}
