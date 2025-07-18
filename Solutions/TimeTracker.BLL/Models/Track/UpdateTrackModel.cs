namespace TimeTracker.BLL.Models.Track;

public class UpdateTrackModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Genre { get; set; }
    public string FilePath { get; set; }
}