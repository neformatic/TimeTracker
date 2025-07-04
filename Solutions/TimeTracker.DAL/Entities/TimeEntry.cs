namespace TimeTracker.DAL.Entities;

public class TimeEntry
{
    public long Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public long UserId { get; set; }

    public User User { get; set; }
}