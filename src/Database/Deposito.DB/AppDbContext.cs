using Deposito.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Deposito.DB;

public class AppDbContext : DbContext
{
    public static string DbConnection = "Data Source=C:\\Users\\Bdimov\\Documents\\GitHub\\Deposito\\src\\Database\\DepositoDB.db;";

    public AppDbContext() {}
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(DbConnection);
    }

    
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Interest> Interests { get; set; }
}