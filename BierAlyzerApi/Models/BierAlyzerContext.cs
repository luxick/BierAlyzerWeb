using Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace BierAlyzerApi.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A bier alyzer context. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BierAlyzerContext : DbContext
    {
        #region Private Properties

        private static bool _created;

        #endregion

        #region Constructor

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="options">  Options for controlling the operation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public BierAlyzerContext(DbContextOptions<BierAlyzerContext> options) : base(options)
        {
            if (!_created)
            {
                _created = true;
                Database.Migrate();
            }
        }

        #endregion

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

        #region Entities

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   User database set </summary>
        /// <value> The user. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<User> User { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event database set </summary>
        /// <value> The event. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<Event> Event { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Drink database set. </summary>
        /// <value> The drink. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<Drink> Drink { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   UserEvent database set </summary>
        /// <value> The user event. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<UserEvent> UserEvent { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   RefreshToken database set </summary>
        /// <value> The refresh token. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<RefreshToken> RefreshToken { get; set; }

        #endregion
    }
}