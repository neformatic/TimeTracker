using AutoMapper;
using TimeTracker.API.ViewModels.Common;
using TimeTracker.API.ViewModels.Track;
using TimeTracker.API.ViewModels.Track.Filter;
using TimeTracker.BLL.Models.Track;
using TimeTracker.BLL.Models.Track.Filter;
using TimeTracker.Common.Models;

namespace TimeTracker.API.Mapping;

public class TrackMapping : Profile
{
    public TrackMapping()
    {
        // BLL <-> API ViewModels
        CreateMap<TrackModel, TrackViewModel>().ReverseMap();
        CreateMap<CreateTrackViewModel, CreateTrackModel>();
        CreateMap<UpdateTrackViewModel, UpdateTrackModel>();

        // Filters
        CreateMap<TrackFilterViewModel, TrackFilterModel>();
        
        // Pagination Result (BLL -> API)
        CreateMap<PaginationResultModel<TrackModel>, PaginationResultViewModel<TrackViewModel>>();
    }
}