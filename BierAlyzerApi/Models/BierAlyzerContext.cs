﻿using Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace BierAlyzerApi.Models
{
    public class BierAlyzerContext : DbContext
    {
        #region Entities

        public DbSet<User> User { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Drink> Drink { get; set; }
        public DbSet<UserEvent> UserEvent { get; set; }

        #endregion

        public BierAlyzerContext(DbContextOptions<BierAlyzerContext> options)
            : base(options)
        {
        }

        #region OnModelCreating

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Executes the model creating action. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="modelBuilder"> The model builder. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEvent>().HasKey(ue => new { ue.UserId, ue.EventId });

            modelBuilder.Entity<DrinkEntry>()
                .HasOne(de => de.Event)
                .WithMany(e => e.DrinkEntries)
                .HasForeignKey(de => de.EventId);

            modelBuilder.Entity<DrinkEntry>()
                .HasOne(de => de.User)
                .WithMany(u => u.DrinkEntries)
                .HasForeignKey(de => de.UserId);

            modelBuilder.Entity<DrinkEntry>()
                .HasOne(de => de.Drink)
                .WithMany(u => u.DrinkEntries)
                .HasForeignKey(de => de.DrinkId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Owner)
                .WithMany(u => u.OwnedEvents)
                .HasForeignKey(e => e.OwnerId);

            modelBuilder.Entity<Drink>()
                .HasOne(d => d.Owner)
                .WithMany(u => u.OwnedDrinks)
                .HasForeignKey(e => e.OwnerId);
        }

        #endregion
    }
}
