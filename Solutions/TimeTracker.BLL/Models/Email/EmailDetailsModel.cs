namespace TimeTracker.BLL.Models.Email;

public class EmailDetailsModel
{
    public List<string> Recipients { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}