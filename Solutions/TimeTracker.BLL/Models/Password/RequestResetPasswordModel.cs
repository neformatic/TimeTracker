namespace TimeTracker.BLL.Models.Password;

public class RequestResetPasswordModel
{
    public string Email { get; set; }
    public string AppUrl { get; set; }
}