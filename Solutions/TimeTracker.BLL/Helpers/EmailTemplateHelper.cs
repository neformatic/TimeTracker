using TimeTracker.BLL.Helpers.Interfaces;
using TimeTracker.BLL.Models.Templates;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Helpers;

public class EmailTemplateHelper : TemplateHelper, IEmailTemplateHelper
{
    private const string ResetPasswordEmailTemplateName = "ResetPasswordEmailTemplate.json";
    private const string UserCredentialsEmailTemplateName = "UserCredentialsEmailTemplate.json";
    private const string EmailTemplatesFolderName = "Email";
    private const string YearDateFormat = "yyyy";

    public async Task<ResetPasswordEmailTemplate> GetResetPasswordEmailTemplateAsync()
    {
        var template = await GetTemplateAsync<ResetPasswordEmailTemplate>(ResetPasswordEmailTemplateName, EmailTemplatesFolderName);

        var currentYear = DateTimeOffset.UtcNow.ToString(YearDateFormat);
        template.Content = template.Content.Replace(TemplatePlaceholderConstants.Year, currentYear);

        return template;
    }

    public async Task<UserCredentialsEmailTemplate> GetUserCredentialsEmailTemplateAsync(UserRole requestedCredentialsUserRole)
    {
        var templateName = requestedCredentialsUserRole switch
        {
            UserRole.DefaultUser => UserCredentialsEmailTemplateName,
            UserRole.SuperAdmin => UserCredentialsEmailTemplateName,
            _ => throw new ArgumentOutOfRangeException(nameof(requestedCredentialsUserRole), requestedCredentialsUserRole, null)
        };

        var template = await GetTemplateAsync<UserCredentialsEmailTemplate>(templateName, EmailTemplatesFolderName);

        var currentYear = DateTimeOffset.UtcNow.ToString(YearDateFormat);
        template.Content = template.Content.Replace(TemplatePlaceholderConstants.Year, currentYear);

        return template;
    }
}