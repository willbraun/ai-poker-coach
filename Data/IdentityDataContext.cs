using ai_poker_coach.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}