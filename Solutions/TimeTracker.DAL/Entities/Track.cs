namespace TimeTracker.DAL.Entities;

public class Track
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Genre { get; set; }
    public string FilePath { get; set; }
}