using System.Text;
using System.Text.Json;
using TimeTracker.BLL.Helpers.Interfaces;

namespace TimeTracker.BLL.Helpers;

public class TemplateHelper : ITemplateHelper
{
    private const string TemplatesFolder = "Templates";
    private const string ResourcesFolder = "Resources";
    private const string TimeTrackerLogoFileName = "timeTrackerLogo.jpg";

    public string ReplacePlaceholders(string template, Dictionary<string, string> placeHoldersDictionary)
    {
        var stringBuilder = new StringBuilder(template);

        foreach (var placeHolder in placeHoldersDictionary)
        {
            stringBuilder.Replace(placeHolder.Key, placeHolder.Value);
        }

        return stringBuilder.ToString();
    }

    public string CombineHtmlParts(params string[] htmlParts)
    {
        var combinedHtml = string.Join(Environment.NewLine, htmlParts);

        return combinedHtml;
    }

    public async Task<string> GetTimeTrackerLogoBase64Async()
    {
        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolder, TimeTrackerLogoFileName);
        var logoBytes = await File.ReadAllBytesAsync(logoPath);
        var logoBase64 = Convert.ToBase64String(logoBytes);

        return logoBase64;
    }

    protected string GenerateFilePath(string entityFolder, string templateName)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplatesFolder, entityFolder, templateName);
    }

    public async Task<T> GetTemplateAsync<T>(string templateName, string templateFolder)
    {
        var templatePath = GenerateFilePath(templateFolder, templateName);
        var fileContent = await File.ReadAllTextAsync(templatePath);
        var template = JsonSerializer.Deserialize<T>(fileContent);

        return template;
    }
}