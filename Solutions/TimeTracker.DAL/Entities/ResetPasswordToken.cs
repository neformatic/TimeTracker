namespace TimeTracker.DAL.Entities;

public class ResetPasswordToken
{
    public long UserId { get; set; }
    public string Token { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public bool IsActive { get; set; }

    public User User { get; set; }
}