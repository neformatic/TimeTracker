using TimeTracker.BLL.Models.Templates;
using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Helpers.Interfaces;

public interface IEmailTemplateHelper : ITemplateHelper
{
    Task<ResetPasswordEmailTemplate> GetResetPasswordEmailTemplateAsync();
    Task<UserCredentialsEmailTemplate> GetUserCredentialsEmailTemplateAsync(UserRole requestedCredentialsUserRole);
}