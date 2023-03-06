namespace FinancialApplication.Service.Implementation;

public class CategoryService : ICategoryService
{
    private readonly FinancialApplicationDbContext _context;

    public CategoryService(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Category category)
    {
        category.DateModified = DateTime.Now;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public Task Delete(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        _context.Categories.Remove(category);
        return _context.SaveChangesAsync();
    }

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
