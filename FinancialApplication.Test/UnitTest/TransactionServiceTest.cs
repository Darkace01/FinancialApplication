using FinancialApplication.Data;
using FinancialApplication.DTO;
using FinancialApplication.Service.Contract;
using FinancialApplication.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialApplication.Test.UnitTest;

public class TransactionServiceTest : TestBase
{
    [Fact]
    public async Task GetTransactions_ReturnsAllTransactions()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTransactions_ReturnsAllTransactions));
        var transactionService = MockTransactionService(dbContext);
        // Act
        var transactions = await transactionService.GetAll();
        // Assert
        Assert.Equal(5, transactions.Count());
    }

    [Fact]
    public async Task GetTransactionById_ReturnsATransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetTransactionById_ReturnsATransaction));
        var transactionService = MockTransactionService(dbContext);
        // Act
        var transaction = await transactionService.Get(1);
        // Assert
        Assert.Equal("Groceries", transaction.Title);
    }

    [Fact]
    public async Task CreateTransaction_ShouldCreateTransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateTransaction_ShouldCreateTransaction));
        var transactionService = MockTransactionService(dbContext);
        // Act
        await transactionService.Add(new TransactionCreateDTO { Title = "Test", Description = "Test", Amount = 100.00M, InFlow = false, CategoryId = 1,DateAdded = "24/05/2023" });
        var transactions = await transactionService.GetAll();
        // Assert
        Assert.Equal(6, transactions.Count());
        Assert.Equal("Test", transactions.Last().Title);
    }

    [Fact]
    public async Task UpdateTransaction_ShouldUpdateTransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdateTransaction_ShouldUpdateTransaction));
        var transactionService = MockTransactionService(dbContext);
        // Act
        await transactionService.Update(new TransactionUpdateDTO { Id = 1, Title = "Test", Description = "Test", Amount = 100.00M, CategoryId = 1 });
        var transaction = await transactionService.Get(1);
        // Assert
        Assert.Equal("Test", transaction.Title);
    }

    [Fact]
    public async Task DeleteTransaction_ShouldDeleteTransaction()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeleteTransaction_ShouldDeleteTransaction));
        var transactionService = MockTransactionService(dbContext);
        // Act
        await transactionService.Delete(1);
        var transactions = await transactionService.GetAll();
        // Assert
        Assert.Equal(4, transactions.Count());
    }


    #region Helpers
    private ITransactionService MockTransactionService(FinancialApplicationDbContext context)
    {
        TransactionService transactionService = new(context);
        return transactionService;
    }
    #endregion
}
