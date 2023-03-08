namespace FinancialApplication.Service.Implementation;

public class UserService
{
    private readonly FinancialApplicationDbContext _context;

    public UserService(FinancialApplicationDbContext context)
    {
        _context = context;
    }


    #region Private Methods
    //private string GenerateUserConfirmationCode(string userId)
    //{
    //    var userConfirmationCode = new UserConfirmationCode
    //    {
    //        Code = Guid.NewGuid().ToString(),
    //        UserId = userId,
    //        ExpiryDate = DateTime.Now.AddMinutes(10)
    //    };

    //}
    #endregion
}
