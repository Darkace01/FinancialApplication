namespace FinancialApplication.DTO;

public class LoginDTO
{
    public string username { get; set; }
    public string password { get; set; }
}


public class LoginResponseDTO
{
    public string accessToken { get; set; }
    public string emailAddress { get; set; }
    public string fullName { get; set; }
    public string userId { get; set; }
}