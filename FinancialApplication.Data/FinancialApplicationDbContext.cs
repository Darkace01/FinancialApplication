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

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Transaction
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId);

        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.Id);
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Title)
            .HasMaxLength(250)
            .IsRequired();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Description)
            .HasMaxLength(500);
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        modelBuilder.Entity<Transaction>()
            .Property(t => t.InFlow)
            .IsRequired();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.CategoryId)
            .IsRequired();

        //Category

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Category>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>()
            .Property(c => c.Title)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Category>()
            .Property(c => c.Description)
            .HasMaxLength(500);
        modelBuilder.Entity<Category>()
            .Property(c => c.Icon)
            .HasMaxLength(100);
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Title)
            .IsUnique();
        base.OnModelCreating(modelBuilder);
    }

}
