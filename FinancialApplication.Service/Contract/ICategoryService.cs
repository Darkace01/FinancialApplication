namespace FinancialApplication.Service.Contract;

public interface ICategoryService
{
    Task Add(Category category);
    Task Delete(int categoryId);
    Task DeleteUserCategory(string userId, int categoryId);
    Task<CategoryDTO> Get(int id);
    Task<IEnumerable<CategoryDTO>> GetAll();
    Task<IEnumerable<CategoryDTO>> GetByUser(string userId);
    Task<CategoryDTO> GetByUserAndCategoryId(string userId, int categoryId);
    Task<bool> IsCategoryExistForUser(string title, string userId);
    Task Update(Category category);
    Task UpdateUserCategory(string userId, CategoryUpdateDTO categoryUpdateDTO);
}
