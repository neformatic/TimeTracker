namespace TimeTracker.API.ViewModels.Common;

public class PaginationResultViewModel<T>
{
    public int ResultsCount { get; set; }
    public int PagesCount { get; set; }
    public int PageNumber { get; set; }
    public List<T> PageItems { get; set; }
}