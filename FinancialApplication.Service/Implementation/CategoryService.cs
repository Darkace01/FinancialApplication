namespace FinancialApplication.Service.Implementation;

public class CategoryService : ICategoryService
{
    private readonly FinancialApplicationDbContext _context;

    public CategoryService(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Add Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public async Task Add(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public async Task Update(Category category)
    {
        category.DateModified = DateTime.Now;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete Category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public Task Delete(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        _context.Categories.Remove(category);
        return _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get Category by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CategoryDTO> Get(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return new CategoryDTO()
        {
            Id = category.Id,
            Title = category.Title,
            Description = category.Description,
            Icon = category.Icon,
        };
    }

    /// <summary>
    /// Get all Categories
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<CategoryDTO>> GetAll()
    {
        return await _context.Categories.AsNoTracking().Select(x => new CategoryDTO()
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            Icon = x.Icon,
        }).ToListAsync();
    }
}
