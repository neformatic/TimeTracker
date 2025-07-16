using FluentValidation;

namespace TimeTracker.API.ViewModels.Password;

public class RequestResetPasswordViewModel
{
    public string Email { get; set; }
}

public class RequestResetPasswordViewModelValidator : AbstractValidator<RequestResetPasswordViewModel>
{
    public RequestResetPasswordViewModelValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty();
    }
}