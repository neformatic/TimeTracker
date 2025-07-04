namespace TimeTracker.BLL.Services.Interfaces;

public interface IEmailService
{
    Task SendAsync(string email, string subject, string message);
    Task SendAsync(List<string> emails, string subject, string message);
}