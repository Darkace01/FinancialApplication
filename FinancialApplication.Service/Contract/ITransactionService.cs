namespace FinancialApplication.Service.Contract;

public interface ITransactionService
{
    Task Add(TransactionCreateDTO transaction);
    Task Delete(int transactionId);
    Task<Transaction> Get(int id);
    Task<IEnumerable<Transaction>> GetAll();
    Task<IEnumerable<Transaction>> GetByCategory(int categoryId);
    Task<TransactionDTO> GetByIdandUserId(int id, string userId);
    Task<IEnumerable<Transaction>> GetByUser(string userId);
    Task<IEnumerable<Transaction>> GetByUserAndCategory(string userId, int categoryId);
    Task<IEnumerable<TransactionDTO>> GetByUserWithParameters(string userId, DateTime startDate, DateTime endDate, int take = 50,string query = "");
    Task Update(TransactionDTO transaction);
}