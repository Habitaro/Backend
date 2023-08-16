using AutoMapper;
using DataAccess.Entities;

namespace WebApi.Models.Services.Helpers
{
    public class HabitDayListToDictionaryConverter : ITypeConverter<IEnumerable<HabitDay>, IDictionary<string, bool>>
    {
        public HabitDayListToDictionaryConverter()
        {
        }

        public IDictionary<string, bool> Convert(IEnumerable<HabitDay> source, IDictionary<string, bool> destination, ResolutionContext context)
        {
            var dictionary = new Dictionary<string, bool>();

            foreach (var habitDay in source)
            {
                dictionary[habitDay.Date.ToString()] = habitDay.IsCompleted;
            }

            return dictionary;
        }
    }
}
