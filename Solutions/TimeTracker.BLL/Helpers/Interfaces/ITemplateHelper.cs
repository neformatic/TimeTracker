namespace TimeTracker.BLL.Helpers.Interfaces;

public interface ITemplateHelper
{
    string ReplacePlaceholders(string template, Dictionary<string, string> placeHoldersDictionary);
    string CombineHtmlParts(params string[] htmlParts);
    Task<string> GetTimeTrackerLogoBase64Async();
    Task<T> GetTemplateAsync<T>(string templateName, string templateFolder);
}