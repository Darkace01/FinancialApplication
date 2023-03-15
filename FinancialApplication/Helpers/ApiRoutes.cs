namespace FinancialApplication.Helpers;

public static class ApiRoutes
{
    public static class AuthRoutes
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string ChangePassword = "change-password";
        public const string ResetPasswordRequest = "reset-password-request";
        public const string ResetPassword = "reset-password";
        public const string ConfirmEmail = "confirm-email";
        public const string ResendConfirmationEmail = "resend-confirmation-email";
    }

    public static class CategoryRoutes
    {
        public const string GetAllCategories = "";
    }

    public static class TransactionRoutes
    {
        public const string GetByUser = "user";
        public const string GetByTransactionIdandUser = "user/{transactionId:int}";
        public const string CreateByUser = "user";
        public const string DeleteByUser = "user/{transactionId:int}";
        public const string UpdateByUser = "user/{transactionId:int}";
        public const string GetUserTransactionBalance = "user/balance";
        public const string GetUserTransactionMonthlyBalance = "user/balance/monthly";
        public const string GetUserDashboard = "user/dashboard";
        
    }
}
