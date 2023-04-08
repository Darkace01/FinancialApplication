namespace FinancialApplication.Service.Implementation;

public class UserService : IUserService
{
    private readonly FinancialApplicationDbContext _context;

    public UserService(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generate a random 6 digit code for user confirmation
    /// </summary>
    /// <param name="userId"></param>
    /// <returns> Confirmation Code </returns>
    public async Task<string> GenerateUserConfirmationCode(string userId)
    {
        var code = CommonHelpers.GenerateRandomNumbers(6).ToString();
        UserConfirmationCode userConfirmationCode = new()
        {
            UserId = userId,
            Code = code,
            ExpiryDate = DateTime.Now.AddMinutes(10)
        };
        _context.UserConfirmationCodes.Add(userConfirmationCode);
        await _context.SaveChangesAsync();

        return code;
    }

    /// <summary>
    /// Check if user confirmation code is valid. Returns true if the code is valid else it returns false
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<bool> VerifyUserConfirmationCode(string userId, string code)
    {
        var userConfirmationCode = await _context.UserConfirmationCodes.FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);
        if (userConfirmationCode == null)
        {
            return false;
        }
        if (userConfirmationCode.ExpiryDate < DateTime.Now)
        {
            return false;
        }

        userConfirmationCode.IsUsed = true;
        _context.UserConfirmationCodes.Update(userConfirmationCode);

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Verify User Email. Returns true if the code is valid else it returns false
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<bool> VerifyUserEmail(string userId, string code)
    {
        var userConfirmationCode = await _context.UserConfirmationCodes.FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);
        if (userConfirmationCode == null)
        {
            return false;
        }
        if (userConfirmationCode.ExpiryDate < DateTime.Now)
        {
            return false;
        }
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        user.EmailConfirmed = true;
        _context.Users.Update(user);

        userConfirmationCode.IsUsed = true;
        _context.UserConfirmationCodes.Update(userConfirmationCode);

        await _context.SaveChangesAsync();
        return true;
    }

}
