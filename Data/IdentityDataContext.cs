using ai_poker_coach.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Action = ai_poker_coach.Models.Domain.Action;

namespace DotNet8Authentication.Data
{
    public class IdentityDataContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Hands)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(e => e.ApplicationUserId)
                .IsRequired();

            modelBuilder.Entity<Hand>()
                .HasMany(e => e.Actions)
                .WithOne(e => e.Hand)
                .HasForeignKey(e => e.HandId)
                .IsRequired();

            modelBuilder.Entity<Hand>()
                .HasMany(e => e.Cards)
                .WithOne(e => e.Hand)
                .HasForeignKey(e => e.HandId)
                .IsRequired();

            modelBuilder.Entity<Hand>()
                .HasMany(e => e.Evaluations)
                .WithOne(e => e.Hand)
                .HasForeignKey(e => e.HandId)
                .IsRequired();

            modelBuilder.Entity<Hand>()
                .HasMany(e => e.Pots)
                .WithOne(e => e.Hand)
                .HasForeignKey(e => e.HandId)
                .IsRequired();

            modelBuilder.Entity<Action>()
                .HasMany(e => e.PotActions)
                .WithOne(e => e.Action)
                .HasForeignKey(e => e.ActionId)
                .IsRequired();

            modelBuilder.Entity<Pot>()
                .HasMany(e => e.PotActions)
                .WithOne(e => e.Pot)
                .HasForeignKey(e => e.PotId)
                .IsRequired();
        }
    }
}