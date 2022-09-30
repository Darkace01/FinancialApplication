namespace FinancialApplication.Service.Contract;

public interface ICategoryService
{
    Task Add(Category category);
    Task Delete(int categoryId);
    Task<Category> Get(int id);
    Task<IEnumerable<Category>> GetAll();
    Task<IEnumerable<Category>> GetByUser(string userId);
    Task Update(Category category);
}
