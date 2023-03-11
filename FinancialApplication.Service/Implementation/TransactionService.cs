using System.Globalization;

namespace FinancialApplication.Service.Implementation;

public class TransactionService : ITransactionService
{
    private readonly FinancialApplicationDbContext _context;
    public TransactionService(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Add a new transaction
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task Add(TransactionCreateDTO model)
    {
        var convertedDate = CommonHelpers.ConvertToDate(model.DateAdded);
        var transaction = new Transaction()
        {
            Amount = model.Amount,
            CategoryId = model.CategoryId,
            DateCreated = convertedDate,
            Description = model.Description,
            UserId = model.UserId,
            InFlow = model.InFlow,
            Title = model.Title
        };
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update a transaction
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public async Task Update(TransactionUpdateDTO transaction)
    {
        var transactionToUpdate = await _context.Transactions.FindAsync(transaction.Id);
        transactionToUpdate.Title = transaction.Title;
        transactionToUpdate.Description = transaction.Description;
        transactionToUpdate.Amount = transaction.Amount;
        transactionToUpdate.CategoryId = transaction.CategoryId;
        transactionToUpdate.DateModified = DateTime.Now;
        transactionToUpdate.DateCreated = transaction.DateAdded.HasValue ? transaction.DateAdded.Value : transactionToUpdate.DateCreated;
        _context.Transactions.Update(transactionToUpdate);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete a transaction
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    public async Task Delete(int transactionId)
    {
        var transaction = _context.Transactions.Find(transactionId);
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a transaction by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Transaction> Get(int id)
    {
        return await _context.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Get all transactions
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Transaction>> GetAll()
    {
        return await _context.Transactions.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Get all transactions by category Id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Transaction>> GetByCategory(int categoryId)
    {
        return await _context.Transactions.Where(e => e.CategoryId == categoryId).AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Get all transactions by user Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Transaction>> GetByUser(string userId)
    {
        return await _context.Transactions.Where(e => e.UserId == userId).AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Get all transactions by user Id with parameters: startDate, endDate, take, query
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="take"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TransactionDTO>> GetByUserWithParameters(string userId, DateTime startDate, DateTime endDate, int take = 50, string query = "")
    {
        var transactions = _context.Transactions.Where(e => e.UserId == userId).Include(e => e.Category).AsNoTracking().AsQueryable();
        
            transactions = transactions.Where(x => x.DateCreated.Date >= startDate.Date);
            transactions = transactions.Where(x => x.DateCreated.Date <= endDate.Date);
       
        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.ToLower().Trim();
            transactions = transactions.Where(x =>
            x.Title.ToLower().Contains(query)
            || x.Description.ToLower().Contains(query)
            || x.Category.Title.ToLower().Contains(query)
            );
        }
        transactions = transactions.OrderByDescending(x => x.DateCreated)
            .Take(take);
        var transactionModel = transactions.Select(x => new TransactionDTO()
        {
            Id = x.Id,
            Amount = x.Amount,
            CategoryId = x.CategoryId,
            DateAdded = x.DateCreated,
            Title = x.Title,
            Description = x.Description,
            UserId = x.UserId,
            CategoryName = x.Category.Title,
            CategoryIcon = x.Category.Icon,
            InFlow = x.InFlow
        });
        return await transactionModel.ToListAsync();
    }

    /// <summary>
    /// Get all transactions by user Id and transaction id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<TransactionDTO> GetByIdandUserId(int id, string userId)
    {
        return await _context.Transactions.Include(x => x.Category).AsNoTracking()
            .Select(transaction => new TransactionDTO()
            {
                Amount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Title,
                CategoryIcon = transaction.Category.Icon,
                DateAdded = transaction.DateCreated,
                Title = transaction.Title,
                Description = transaction.Description,
                Id = transaction.Id,
                UserId = transaction.UserId,
                InFlow = transaction.InFlow
            })
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    }
    /// <summary>
    /// Get all transactions by user Id and category Id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>

    public async Task<IEnumerable<Transaction>> GetByUserAndCategory(string userId, int categoryId)
    {
        return await _context.Transactions.Where(e => e.UserId == userId && e.CategoryId == categoryId).AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Get user balance for the current month
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public async Task<ClientTransactionBalance> GetUserBalanceForTheMonth(string userId, DateTime date)
    {
        var transactions = await _context.Transactions.Where(e => e.UserId == userId).AsNoTracking().ToListAsync();
        var totalInflow = transactions.Where(x => x.InFlow == true && x.DateCreated.Month == date.Month && x.DateCreated.Year == date.Year).Sum(x => x.Amount);
        var totalOutflow = transactions.Where(x => x.InFlow == false && x.DateCreated.Month == date.Month && x.DateCreated.Year == date.Year).Sum(x => x.Amount);
        var balance = totalInflow - totalOutflow;
        //with percentage
        var percentage = (balance == 0 || totalInflow == 0) ? 0 : Math.Round((balance / totalInflow) * 100,2);

        return new ClientTransactionBalance()
        {
            Balance = balance,
            TotalInflow = totalInflow,
            TotalOutflow = totalOutflow,
            Percentage = percentage            
        };
    }

    /// <summary>
    /// Get user balance for every month from January to December
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<ClientTransactionMonthlyBalance>> GetUserBalanceForEveryMonthFromJanuaryToDecember(string userId)
    {
        var dateRange = Enumerable.Range(1, 12).Select(x => new DateTime(DateTime.Now.Year, x, 1)).ToList();
        var transactions = await _context.Transactions.Where(e => e.UserId == userId).AsNoTracking().Select(x => new
        {
            x.Amount,
            x.DateCreated,
            x.InFlow
        }).ToListAsync();
        var balanceList = new List<ClientTransactionMonthlyBalance>();
        foreach (var date in dateRange)
        {
            var totalInflow = transactions.Where(x => x.InFlow == true && x.DateCreated.Month == date.Month && x.DateCreated.Year == date.Year).Sum(x => x.Amount);
            var totalOutflow = transactions.Where(x => x.InFlow == false && x.DateCreated.Month == date.Month && x.DateCreated.Year == date.Year).Sum(x => x.Amount);
            var balance = totalInflow - totalOutflow;
            //with percentage
            var percentage = (balance == 0 || totalInflow == 0) ? 0 : Math.Round((balance / totalInflow) * 100, 2);
            var monthInWord = date.ToString("MMM", CultureInfo.InvariantCulture);

            balanceList.Add(new ClientTransactionMonthlyBalance()
            {
                Balance = balance,
                Percentage = percentage,
                Month = monthInWord
            });
        }
        return balanceList;
    }
}
