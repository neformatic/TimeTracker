using FluentValidation;

namespace TimeTracker.API.ViewModels.Password;

public class SetPasswordViewModel
{
    public string Token { get; set; }
    public string Password { get; set; }
}

public class SetPasswordViewModelValidator : AbstractValidator<SetPasswordViewModel>
{
    public SetPasswordViewModelValidator()
    {
        RuleFor(m => m.Token)
            .NotEmpty();

        RuleFor(m => m.Password)
            .NotEmpty();
    }
}