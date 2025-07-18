namespace TimeTracker.Common.Constants;

public class ErrorMessageConstants
{
    public const string SomethingWentWrong = "Something went wrong";
    public const string InvalidPasswordOrEmail = "Invalid password or email";
    public const string InvalidJwtToken = "Jwt token is invalid";
    public const string InvalidRefreshToken = "Refresh token is invalid";
    public const string InactiveUserLogin = "You're deactivated";
    public const string UserWithThatEmailNotExist = "Invalid email address";
    public const string PasswordDoesntMatchValidation = "Password does not match the password requirements";
    public const string UserNotFound = "User was not found";
    public const string EmailSendLimitExceeded = "Email hasn't been sent. It's possible to send one email for one User within 1 hour";
    public const string PasswordTokenInvalidOrExpired = "Link is not active anymore";
    public const string NewPasswordIsEqualToPrevious = "New password should not match your previous password for this account";
    public const string ResetPasswordTokenInvalidOrExpired = "Link is not active anymore, please, go to authorization page and try again";
    public const string SetPasswordTokenInvalidOrExpiredTemplate = "Link is not active anymore";
    public const string ContactEmailAlreadyInUse = "Email should be unique";
    public const string TrackAlreadyExists = "Track should be unique";
}