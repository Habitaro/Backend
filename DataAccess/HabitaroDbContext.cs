using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HabitaroDbContext : DbContext
    {
        public HabitaroDbContext(DbContextOptions<HabitaroDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "HabitaroDb");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Rank>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<User>()
                .HasOne<Rank>().WithMany(r => r.Users).HasForeignKey(u => u.RankId);

            modelBuilder.Entity<Rank>().HasData(new Rank()
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
            });

            base.OnModelCreating(modelBuilder);
        }
    }   
}
