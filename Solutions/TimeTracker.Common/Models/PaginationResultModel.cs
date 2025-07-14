namespace TimeTracker.Common.Models;

public class PaginationResultModel<T>
{
    public long ResultsCount { get; set; }
    public int PagesCount { get; set; }
    public int PageNumber { get; set; }
    public List<T> PageItems { get; set; }
}