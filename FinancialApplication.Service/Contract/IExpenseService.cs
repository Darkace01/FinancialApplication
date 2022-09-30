namespace FinancialApplication.Service.Contract;

public interface IExpenseService
{
    Task Add(Expense expense);
    Task Delete(int expenseId);
    Task<Expense> Get(int id);
    Task<IEnumerable<Expense>> GetAll();
    Task<IEnumerable<Expense>> GetByCategory(int categoryId);
    Task<IEnumerable<Expense>> GetByUser(string userId);
    Task<IEnumerable<Expense>> GetByUserAndCategory(string userId, int categoryId);
    Task Update(Expense expense);
}