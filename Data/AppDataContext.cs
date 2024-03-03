using ai_poker_coach.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
  }
}