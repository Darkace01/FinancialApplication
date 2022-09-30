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

    public async Task<Category> Get(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetByUser(string userId)
    {
        return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
    }


}
