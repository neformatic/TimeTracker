using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeTracker.BLL.Models.Track;
using TimeTracker.BLL.Models.Track.Filter;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Exceptions;
using TimeTracker.Common.Helpers.Interfaces;
using TimeTracker.Common.Models;
using TimeTracker.DAL;
using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Extensions;

namespace TimeTracker.BLL.Services;

public class TrackService : ITrackService
{
    private readonly TimeTrackerDbContext _dbContext;
    private readonly IPaginationHelper _paginationHelper;
    private readonly IMapper _mapper;

    public TrackService(TimeTrackerDbContext dbContext,
        IPaginationHelper paginationHelper,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _paginationHelper = paginationHelper;
        _mapper = mapper;
    }

    public async Task<TrackModel> CreateAsync(CreateTrackModel createTrackModel)
    {
        var createTrackEntity = _mapper.Map<Track>(createTrackModel);

        await _dbContext.ExecuteInTransactionAsync(async () =>
        {
            _dbContext.Tracks.Add(createTrackEntity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.CheckPostgresUniqueConstraintException())
            {
                throw new BadRequestException(ErrorMessageConstants.TrackAlreadyExists);
            }
        });
        
        var createdTrackModel = _mapper.Map<TrackModel>(createTrackEntity);
        return createdTrackModel;
    }

    public async Task<TrackModel> UpdateAsync(UpdateTrackModel updateTrackModel)
    {
        var track = await _dbContext.Tracks
            .FirstOrDefaultAsync(t => t.Id == updateTrackModel.Id);

        if (track is null)
        {
            throw new EntityNotFoundException(updateTrackModel.Id);
        }

        _mapper.Map(updateTrackModel, track);

        await _dbContext.ExecuteInTransactionAsync(async () =>
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.CheckPostgresUniqueConstraintException())
            {
                throw new BadRequestException(ErrorMessageConstants.TrackAlreadyExists);
            }
        });

        var trackModel = _mapper.Map<TrackModel>(track);
        return trackModel;
    }

    public async Task<TrackModel> GetAsync(long id)
    {
        var trackModel = await _dbContext.Tracks
            .Where(t => t.Id == id)
            .Select(t => _mapper.Map<TrackModel>(t))
            .FirstOrDefaultAsync();

        if (trackModel is null)
        {
            throw new EntityNotFoundException(id);
        }

        return trackModel;
    }

    public async Task<PaginationResultModel<TrackModel>> GetTracksListAsync(TrackFilterModel filter)
    {
        var query = _dbContext.Tracks.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(t => t.Title.Contains(filter.Title));
        }
        if (!string.IsNullOrWhiteSpace(filter.Artist))
        {
            query = query.Where(t => t.Artist.Contains(filter.Artist));
        }
        if (!string.IsNullOrWhiteSpace(filter.Genre))
        {
            query = query.Where(t => t.Genre.Contains(filter.Genre));
        }

        var trackModels = await query
            .Select(t => _mapper.Map<TrackModel>(t))
            .ToListAsync();

        var paginationResult = _paginationHelper.GeneratePaginationResultModel(
            trackModels,
            filter.PageNumber,
            filter.PageItemsCount
        );

        return paginationResult;
    }

    public async Task DeleteAsync(long id)
    {
        var track = await _dbContext.Tracks.FirstOrDefaultAsync(t => t.Id == id);

        if (track is null)
        {
            throw new EntityNotFoundException(id);
        }

        await _dbContext.ExecuteInTransactionAsync(async () =>
        {
            _dbContext.Tracks.Remove(track);
            await _dbContext.SaveChangesAsync();
        });
    }
}