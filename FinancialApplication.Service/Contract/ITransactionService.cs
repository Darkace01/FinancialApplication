namespace FinancialApplication.Service.Contract;

public interface ITransactionService
{
    /// <summary>
    /// Add a new transaction
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task Add(TransactionCreateDTO transaction);

    /// <summary>
    /// Delete a transaction
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    Task Delete(int transactionId);

    /// <summary>
    /// Get a transaction by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Transaction> Get(int id);

    /// <summary>
    /// Get all transactions
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Transaction>> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all transactions by category Id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<IEnumerable<Transaction>> GetByCategory(int categoryId);

    /// <summary>
    /// Get all transactions by user Id and transaction id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<TransactionDTO> GetByIdandUserId(int id, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all transactions by user Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Transaction>> GetByUser(string userId);

    /// <summary>
    /// Get user balance for the current month
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<IEnumerable<Transaction>> GetByUserAndCategory(string userId, int categoryId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Get all transactions by user Id with parameters: startDate, endDate, take, query
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="take"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<TransactionDTO>> GetByUserWithParameters(string userId, DateTime startDate, DateTime endDate, int take = 50,string query = "");

    /// <summary>
    /// Get user balance for every month from January to December
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<ClientTransactionMonthlyBalance>> GetUserBalanceForEveryMonthFromJanuaryToDecember(string userId,CancellationToken cancellationToken = default);

    /// <summary>
    /// Get user balance for the current month
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<ClientTransactionBalance> GetUserBalanceForTheMonth(string userId, DateTime date, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a transaction
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    Task Update(TransactionUpdateDTO transaction);
}