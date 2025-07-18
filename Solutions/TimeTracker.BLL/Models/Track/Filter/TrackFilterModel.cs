using TimeTracker.Common.Models;

namespace TimeTracker.BLL.Models.Track.Filter;

public class TrackFilterModel : BasePaginationFilterModel
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Genre { get; set; }
}