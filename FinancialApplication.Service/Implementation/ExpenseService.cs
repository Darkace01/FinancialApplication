namespace FinancialApplication.Service.Implementation;

public class ExpenseService : IExpenseService
{
    private readonly FinancialApplicationDbContext _context;
    public ExpenseService(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(ExpenseCreateDTO model)
    {
        var expense = new Expense()
        {
            Amount = model.Amount,
            CategoryId = model.CategoryId,
            DateCreated = DateTime.Now,
            Description = model.Description,
            UserId = model.UserId
        };
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ExpenseDTO expense)
    {
        var expenseToUpdate = await _context.Expenses.FindAsync(expense.Id);
        expenseToUpdate.Title = expense.Title;
        expenseToUpdate.Description = expense.Description;
        expenseToUpdate.Amount = expense.Amount;
        expenseToUpdate.CategoryId = expense.CategoryId;
        expenseToUpdate.DateModified = DateTime.Now;
        _context.Expenses.Update(expenseToUpdate);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int expenseId)
    {
        var expense = _context.Expenses.Find(expenseId);
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }

    public async Task<Expense> Get(int id)
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Expense>> GetAll()
    {
        return await _context.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByCategory(int categoryId)
    {
        return await _context.Expenses.Where(e => e.CategoryId == categoryId).AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByUser(string userId)
    {
        return await _context.Expenses.Where(e => e.UserId == userId).AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<ExpenseDTO>> GetByUserWithParameters(string userId,  DateTime? startDate, DateTime? endDate, int take = 50)
    {
        var expenses = _context.Expenses.Where(e => e.UserId == userId).Include(e => e.Category).AsNoTracking().AsQueryable();
        if (startDate.HasValue)
        {
            expenses = expenses.Where(x => x.DateCreated >= startDate);
        }
        if (endDate.HasValue)
        {
            expenses = expenses.Where(x => x.DateCreated <= endDate);
        }
        expenses = expenses.Take(take);
        var expenseModel = expenses.Select(x => new ExpenseDTO()
        {
            Id = x.Id,
            Amount = x.Amount,
            CategoryId = x.CategoryId,
            DateAdded = x.DateCreated,
            Title = x.Title,
            Description = x.Description,
            UserId = x.UserId,
            CategoryName = x.Category.Title
        });
        return await expenseModel.ToListAsync();
    }

    public async Task<ExpenseDTO> GetByIdandUserId(int id, string userId)
    {
        return await _context.Expenses.Include(x => x.Category).AsNoTracking()
            .Select(expense => new ExpenseDTO()
            {
                Amount = expense.Amount,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category.Title,
                DateAdded = expense.DateCreated,
                Title = expense.Title,
                Description = expense.Description,
                Id = expense.Id,
                UserId = expense.UserId
            })
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    }

    public async Task<IEnumerable<Expense>> GetByUserAndCategory(string userId, int categoryId)
    {
        return await _context.Expenses.Where(e => e.UserId == userId && e.CategoryId == categoryId).AsNoTracking().ToListAsync();
    }
}
