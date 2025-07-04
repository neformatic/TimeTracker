namespace TimeTracker.Common.Models.Settings;

public class EmailSettings
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public string SenderName { get; set; }
    public bool UseSsl { get; set; }
}