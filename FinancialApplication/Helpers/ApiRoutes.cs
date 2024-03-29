﻿namespace FinancialApplication.Helpers;

public static class ApiRoutes
{
    public static class AuthRoutes
    {
        public const string _login = "login";
        public const string _register = "register";
        public const string _changePassword = "change-password";
        public const string _resetPasswordRequest = "reset-password-request";
        public const string _resetPassword = "reset-password";
        public const string _confirmEmail = "confirm-email";
        public const string _resendConfirmationEmail = "resend-confirmation-email";
        public const string _registerOrLoginWithGoogle = "external/google";
    }

    public static class CategoryRoutes
    {
        public const string _getAllCategories = "";
    }

    public static class TransactionRoutes
    {
        public const string _getByUser = "user";
        public const string _getByTransactionIdandUser = "user/{transactionId:int}";
        public const string _createByUser = "user";
        public const string _deleteByUser = "user/{transactionId:int}";
        public const string _updateByUser = "user/{transactionId:int}";
        public const string _getUserTransactionBalance = "user/balance";
        public const string _getUserTransactionMonthlyBalance = "user/balance/monthly";
        public const string _getUserDashboard = "user/dashboard";        
    }

    public static class UserRoutes
    {
        public const string _uploadUserProfilePicture = "profile-picture";
        public const string _userBasicDetails = "basic";
        public const string _userUpdateDetails = "details";
    }

    public static class NotificationRoutes
    {
        public const string _saveUserNotificationToken = "token";
        public const string _turnOnNotification = "turn-on";
        public const string _turnOffNotification = "turn-off";
        public const string _sendTestNotificationToAllUsers = "test";
    }
}
