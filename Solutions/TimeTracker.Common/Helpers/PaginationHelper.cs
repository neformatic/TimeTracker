using Microsoft.EntityFrameworkCore;
using TimeTracker.Common.Helpers.Interfaces;
using TimeTracker.Common.Models;

namespace TimeTracker.Common.Helpers;

public class PaginationHelper : IPaginationHelper
{
    public async Task<PaginationResultModel<T>> GeneratePaginationResultModelAsync<T>(IQueryable<T> items, int pageNumber, int pageItemsCount)
    {
        var totalItemsCount = await items.CountAsync();

        var paginatedItems = await items
            .Skip((pageNumber - 1) * pageItemsCount)
            .Take(pageItemsCount)
            .ToListAsync();

        return new PaginationResultModel<T>
        {
            ResultsCount = totalItemsCount,
            PagesCount = CalculatePagesCount(totalItemsCount, pageItemsCount),
            PageNumber = pageNumber,
            PageItems = paginatedItems
        };
    }

    public PaginationResultModel<T> GeneratePaginationResultModel<T>(List<T> items, int pageNumber, int pageItemsCount)
    {
        var totalItemsCount = items.Count;

        var paginatedItems = items
            .Skip((pageNumber - 1) * pageItemsCount)
            .Take(pageItemsCount)
            .ToList();

        return new PaginationResultModel<T>
        {
            ResultsCount = totalItemsCount,
            PagesCount = CalculatePagesCount(totalItemsCount, pageItemsCount),
            PageNumber = pageNumber,
            PageItems = paginatedItems
        };
    }

    public int CalculatePagesCount(long totalItemsCount, int pageItemsCount)
    {
        return (int)Math.Ceiling((double)totalItemsCount / pageItemsCount);
    }
}