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
            IsSubcategory = category.IsSubcategory,
            Icon = category.Icon,
            UserId = category.UserId
        };
    }

    public async Task<IEnumerable<CategoryDTO>> GetAll()
    {
        return await _context.Categories.AsNoTracking().Select(x => new CategoryDTO()
        {
            Id = x.Id,
            Description = x.Description,
            IsSubcategory = x.IsSubcategory,
            Title = x.Title,
            Icon = x.Icon,
            UserId = x.UserId
        }).ToListAsync();
    }

    public async Task<IEnumerable<CategoryDTO>> GetByUser(string userId)
    {
        return await _context.Categories.Where(c => c.UserId == userId).AsNoTracking().Select(x => new CategoryDTO()
        {
            Id = x.Id,
            Description = x.Description,
            IsSubcategory = x.IsSubcategory,
            Title = x.Title,
            Icon = x.Icon,
            UserId = x.UserId
        }).ToListAsync();
    }

    public async Task<bool> IsCategoryExistForUser(string title, string userId)
    {
        return await _context.Categories.AsNoTracking().AnyAsync(c => c.Title == title && c.UserId == userId);
    }

    public async Task DeleteUserCategory(string userId, int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category.UserId == userId)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CategoryDTO> GetByUserAndCategoryId(string userId, int categoryId)
    {
        var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == categoryId);
        if (category.UserId == userId)
        {
            return new CategoryDTO()
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description,
                IsSubcategory = category.IsSubcategory,
                UserId = category.UserId
            };
        }
        return null;
    }

    public async Task UpdateUserCategory(string userId, CategoryUpdateDTO categoryUpdateDTO)
    {
        var category = await _context.Categories.FindAsync(categoryUpdateDTO.Id);
        if (category.UserId == userId)
        {
            category.Title = categoryUpdateDTO.Title;
            category.Description = categoryUpdateDTO.Description;
            category.IsSubcategory = categoryUpdateDTO.IsSubcategory;
            category.DateModified = DateTime.Now;
            category.Icon = categoryUpdateDTO.Icon;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
