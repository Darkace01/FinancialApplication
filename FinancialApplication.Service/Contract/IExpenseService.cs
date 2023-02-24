namespace FinancialApplication.Service.Contract;

public interface IExpenseService
{
    Task Add(ExpenseCreateDTO expense);
    Task Delete(int expenseId);
    Task<Expense> Get(int id);
    Task<IEnumerable<Expense>> GetAll();
    Task<IEnumerable<Expense>> GetByCategory(int categoryId);
    Task<ExpenseDTO> GetByIdandUserId(int id, string userId);
    Task<IEnumerable<Expense>> GetByUser(string userId);
    Task<IEnumerable<Expense>> GetByUserAndCategory(string userId, int categoryId);
    Task<IEnumerable<ExpenseDTO>> GetByUserWithParameters(string userId, DateTime? startDate, DateTime? endDate, int take = 50);
    Task Update(ExpenseDTO expense);
}