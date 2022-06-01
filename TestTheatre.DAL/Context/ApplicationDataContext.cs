using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheatre.DAL.Entities;

namespace TestTheatre.DAL.Context
{
    public class ApplicationDataContext : DbContext
    {


        public DbSet<User> Users { get; set; }
        public DbSet<UserShows> UserShows { get; set; }
        public DbSet<Show> Shows{ get; set; } 
         
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {

        } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserShows>().HasKey(f =>  new { f.ShowId, f.UserId });
            modelBuilder.Entity<UserShows>()
              .HasOne(b => b.User)
              .WithMany(f => f.UserShows)
              .HasForeignKey(b => b.UserId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserShows>()
              .HasOne(b => b.Show)
              .WithMany(f => f.UserShows)
              .HasForeignKey(b => b.ShowId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
