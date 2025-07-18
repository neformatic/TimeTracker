using TimeTracker.BLL.Models.Track;
using TimeTracker.BLL.Models.Track.Filter;
using TimeTracker.Common.Models;

namespace TimeTracker.BLL.Services.Interfaces;

public interface ITrackService
{
    Task<TrackModel> CreateAsync(CreateTrackModel createTrackModel);
    Task<TrackModel> UpdateAsync(UpdateTrackModel updateTrackModel);
    Task<TrackModel> GetAsync(long id);
    Task<PaginationResultModel<TrackModel>> GetTracksListAsync(TrackFilterModel filter);
    Task DeleteAsync(long id);
}