using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.ViewModels.Common;
using TimeTracker.API.ViewModels.Track;
using TimeTracker.API.ViewModels.Track.Filter;
using TimeTracker.BLL.Models.Track;
using TimeTracker.BLL.Models.Track.Filter;
using TimeTracker.BLL.Services.Interfaces;

namespace TimeTracker.API.Controllers;

public class TrackController : ApiControllerBase
{
    private readonly ITrackService _trackService;
    private readonly IMapper _mapper;

    public TrackController(ITrackService trackService, IMapper mapper)
    {
        _trackService = trackService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(long id)
    {
        var trackModel = await _trackService.GetAsync(id);
        var trackViewModel = _mapper.Map<TrackViewModel>(trackModel);

        return Ok(trackViewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTrackViewModel createTrackViewModel)
    {
        var createTrackModel = _mapper.Map<CreateTrackModel>(createTrackViewModel);
        var createdTrackModel = await _trackService.CreateAsync(createTrackModel);
        var trackViewModel = _mapper.Map<TrackViewModel>(createdTrackModel);

        return Ok(trackViewModel);
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateTrackViewModel updateTrackViewModel)
    {
        var updateTrackModel = _mapper.Map<UpdateTrackModel>(updateTrackViewModel);
        var trackModel = await _trackService.UpdateAsync(updateTrackModel);
        var trackViewModel = _mapper.Map<TrackViewModel>(trackModel);

        return Ok(trackViewModel);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTracksList([FromQuery] TrackFilterViewModel filter)
    {
        var trackFilterModel = _mapper.Map<TrackFilterModel>(filter);
        var tracksList = await _trackService.GetTracksListAsync(trackFilterModel);
        var tracksListViewModel = _mapper.Map<PaginationResultViewModel<TrackViewModel>>(tracksList);

        return Ok(tracksListViewModel);
    }

    [AllowAnonymous]
    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        await _trackService.DeleteAsync(id);
        return NoContent();
    }
}