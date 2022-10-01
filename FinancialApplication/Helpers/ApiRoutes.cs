namespace FinancialApplication.Helpers;

public static class ApiRoutes
{
    public static class AuthRoutes
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string ChangePassword = "change-password";
        public const string ResetPasswordRequest = "request-password-change";
        public const string ResetPassword = "request-password";
    }

    public static class CategoryRoutes
    {
        public const string GetByUser = "by-user";
    }
}
