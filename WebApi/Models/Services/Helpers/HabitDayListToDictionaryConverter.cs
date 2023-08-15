using AutoMapper;
using DataAccess.Entities;

namespace WebApi.Models.Services.Helpers
{
    public class HabitDayListToDictionaryConverter : ITypeConverter<IEnumerable<HabitDay>, IDictionary<DateOnly, bool>>
    {
        public IDictionary<DateOnly, bool> Convert(IEnumerable<HabitDay> source, IDictionary<DateOnly, bool> destination, ResolutionContext context)
        {
            var dictionary = new Dictionary<DateOnly, bool>();

            foreach (var habitDay in source)
            {
                dictionary[DateOnly.FromDateTime(habitDay.Date)] = habitDay.IsCompleted;
            }

            return dictionary;
        }
    }
}
