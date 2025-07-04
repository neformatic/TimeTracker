namespace TimeTracker.DAL.Entities;

public class UserRefreshToken
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }

    public virtual User User { get; set; }
}