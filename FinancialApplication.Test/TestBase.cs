using FinancialApplication.Data;
using FinancialApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialApplication.Test;

public class TestBase
{
    protected FinancialApplicationDbContext GetSampleData(string dbName)
    {
        var options = new DbContextOptionsBuilder<FinancialApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        var dbContext = new FinancialApplicationDbContext(options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // Add sample data
        var categories = new List<Category>
        {
            new Category { Title = "Food" ,Description = "This covers everything relating to food"},
            new Category { Title = "Transportation" ,Description = "This covers everything relating to transportation"},
            new Category { Title = "Entertainment" ,Description = "This covers everything relating to entertainment"},
            new Category { Title = "Utilities" ,Description = "This covers everything relating to utilities"},
            new Category { Title = "Miscellaneous" ,Description = "This covers everything else"}
        };

        var transactions = new List<Transaction>
        {
            new Transaction { Title = "Groceries", Description = "Groceries for the week", Amount = 100.00M, InFlow = false, Category = categories[0]},
            new Transaction { Title = "Gas", Description = "Gas for the car", Amount = 50.00M, InFlow = false, Category = categories[1]},
            new Transaction { Title = "Movie", Description = "Movie with friends", Amount = 20.00M, InFlow = false, Category = categories[2]},
            new Transaction { Title = "Electricity", Description = "Electricity bill", Amount = 200.00M, InFlow = false, Category = categories[3]},
            new Transaction { Title = "Misc", Description = "Miscellaneous", Amount = 50.00M, InFlow = false, Category = categories[4]},
        }; 
        
        dbContext.Categories.AddRange(categories);
        dbContext.Transactions.AddRange(transactions);

        dbContext.SaveChanges();
        
        return dbContext;
    }
}
