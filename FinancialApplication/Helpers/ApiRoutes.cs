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
        public const string GetByUser = "user";
        public const string CreateByUser = "user";
        public const string DeleteByUser = "user/{categoryId:int}";
        public const string UpdateByUser = "user/{categoryId:int}";
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
        
    }
}
