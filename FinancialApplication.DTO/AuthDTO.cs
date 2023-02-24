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
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string phoneNumber { get; set; }
    public string fullName { get { return $"{firstName} {lastName}"; } }
    public string userId { get; set; }
    public string profilePictureUrl { get; set; }
}

public class RegisterDTO
{
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string phoneNumber { get; set; }
}

public class ChangePasswordDTO
{
    public string username { get; set; }
    public string currentPassword { get; set; }
    public string newPassword { get; set; }
}

public class PasswordRequestDTO
{
    public string email { get; set; }    
}

public class PasswordRequestCodeDTO
{
    public string email { get; set; }
    public string code { get; set; }
    public string password { get; set; }
}