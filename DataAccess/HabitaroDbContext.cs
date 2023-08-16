using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HabitaroDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        protected HabitaroDbContext()
        {
        }

        public HabitaroDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Habit> Habits { get; set; }

        public DbSet<HabitDay> HabitDays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Rank>().HasData(new Rank()
            {
                Id = 1,
                Name = "Bronze cat 1",
            }, new Rank()
            {
                Id = 2,
                Name = "Bronze cat 2",
            }, new Rank()
            {
                Id = 3,
                Name = "Bronze cat 3",
            }, new Rank()
            {
                Id = 4,
                Name = "Silver cat 1",
            }, new Rank()
            {
                Id = 5,
                Name = "Silver cat 2",
            }, new Rank()
            {
                Id = 6,
                Name = "Silver cat 3",
            }, new Rank()
            {
                Id = 7,
                Name = "Gold cat 1",
            }, new Rank()
            {
                Id = 8,
                Name = "Gold cat 2",
            }, new Rank()
            {
                Id = 9,
                Name = "Gold cat 3"
            });
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>();
        }
    }
}
