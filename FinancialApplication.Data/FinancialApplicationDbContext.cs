using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinancialApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace FinancialApplication.Data;

public class FinancialApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public FinancialApplicationDbContext(DbContextOptions<FinancialApplicationDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
}
