namespace FinancialApplication.Service.Implementation;

public class ExpenseService : IExpenseService
{
    private readonly FinancialApplicationDbContext _context;
    public ExpenseService(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Expense expense)
    {
        expense.DateModified = DateTime.Now;
        _context.Expenses.Update(expense);
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
        return await _context.Expenses.FindAsync(id);
    }

    public async Task<IEnumerable<Expense>> GetAll()
    {
        return await _context.Expenses.ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByCategory(int categoryId)
    {
        return await _context.Expenses.Where(e => e.CategoryId == categoryId).ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByUser(string userId)
    {
        return await _context.Expenses.Where(e => e.UserId == userId).ToListAsync();
    }

    public async Task<Expense> GetByIdandUserId(int id, string userId)
    {
        return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    }

    public async Task<IEnumerable<Expense>> GetByUserAndCategory(string userId, int categoryId)
    {
        return await _context.Expenses.Where(e => e.UserId == userId && e.CategoryId == categoryId).ToListAsync();
    }
}
