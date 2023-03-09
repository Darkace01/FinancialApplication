namespace FinancialApplication.Service.Contract;

public interface ICategoryService
{
    /// <summary>
    /// Add Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task Add(Category category);
    /// <summary>
    /// Delete Category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task Delete(int categoryId);

    /// <summary>
    /// Get Category by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CategoryDTO> Get(int id);
    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<CategoryDTO>> GetAll();
    /// <summary>
    /// Update Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task Update(Category category);
}
