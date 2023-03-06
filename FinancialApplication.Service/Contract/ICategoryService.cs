namespace FinancialApplication.Service.Contract;

public interface ICategoryService
{
    Task Add(Category category);
    Task Delete(int categoryId);
    Task<CategoryDTO> Get(int id);
    Task<IEnumerable<CategoryDTO>> GetAll();
    Task Update(Category category);
}
