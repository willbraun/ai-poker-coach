using ai_poker_coach.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Action = ai_poker_coach.Data.Action;

namespace PostgreSQL.Data
{
    public class AppDataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Hand> Hands { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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