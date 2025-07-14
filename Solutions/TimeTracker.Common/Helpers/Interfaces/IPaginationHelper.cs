using TimeTracker.Common.Models;

namespace TimeTracker.Common.Helpers.Interfaces;

public interface IPaginationHelper
{
    int CalculatePagesCount(long totalItemsCount, int pageItemsCount);
    Task<PaginationResultModel<T>> GeneratePaginationResultModelAsync<T>(IQueryable<T> items, int pageNumber, int pageItemsCount);
    PaginationResultModel<T> GeneratePaginationResultModel<T>(List<T> items, int pageNumber, int pageItemsCount);
}