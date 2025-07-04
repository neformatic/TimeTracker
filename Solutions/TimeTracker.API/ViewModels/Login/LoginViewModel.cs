using FluentValidation;

namespace TimeTracker.API.ViewModels.Login;

public class LoginViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
    }
}