using Legacy_Todo.Controllers;
using Legacy_Todo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Legacy_Todo.Repos
{
    public class TaskContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            ConfigureModelBuilderForUser(modelBuilder);
        }

        void ConfigureModelBuilderForUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "Demo",
                Password = "Demo".GetHashCode().ToString()
            }); ;
            ;
        }
    }
}
