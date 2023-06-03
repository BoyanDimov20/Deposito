using Deposito.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Deposito.DB;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Interest> Interests { get; set; }
}