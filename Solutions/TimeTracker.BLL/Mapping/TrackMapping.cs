using AutoMapper;
using TimeTracker.BLL.Models.Track;
using TimeTracker.DAL.Entities;

namespace TimeTracker.BLL.Mapping;

public class TrackMapping : Profile
{
    public TrackMapping()
    {
        CreateMap<Track, TrackModel>().ReverseMap();
        CreateMap<CreateTrackModel, Track>();
        CreateMap<UpdateTrackModel, Track>();
    }
}