using FinancialApplication.Data;
using FinancialApplication.Models;
using FinancialApplication.Service.Contract;
using FinancialApplication.Service.Implementation;
using Xunit;

namespace FinancialApplication.Test.UnitTest;

public class CourseServiceTest : TestBase
{
    [Fact]
    public async Task GetCategories_ReturnsAllCategories()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetCategories_ReturnsAllCategories));
        var categoryService = MockCategoryService(dbContext);
        // Act
        var categories = await categoryService.GetAll();
        // Assert
        Assert.Equal(5, categories.Count());
    }

    [Fact]
    public async Task GetCategoryById_ReturnsACategory()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetCategoryById_ReturnsACategory));
        var categoryService = MockCategoryService(dbContext);
        // Act
        var category = await categoryService.Get(1);
        // Assert
        Assert.Equal("Food", category.Title);
    }

    [Fact]
    public async Task CreateCategory_ShouldCreateCategory()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreateCategory_ShouldCreateCategory));
        var categoryService = MockCategoryService(dbContext);
        // Act
        await categoryService.Add(new Category { Title = "Test", Description = "Test" });
        var categories = await categoryService.GetAll();
        // Assert
        Assert.Equal(6, categories.Count());
        Assert.Equal("Test", categories.Last().Title);
    }


    #region Helpers
    private ICategoryService MockCategoryService(FinancialApplicationDbContext context)
    {
        CategoryService categoryService = new(context);
        return categoryService;
    }
    #endregion
}
