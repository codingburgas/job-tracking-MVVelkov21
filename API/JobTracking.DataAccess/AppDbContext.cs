using JobTracking.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace JobTracking.DataAccess;

public class AppDbContext : DbContext
{
    public DbSet<Temp> Laina { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestDb;Integrated Security=True;");
        }
    }
}