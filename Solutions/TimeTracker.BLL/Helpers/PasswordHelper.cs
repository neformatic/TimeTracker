using System.Text.RegularExpressions;
using TimeTracker.BLL.Helpers.Interfaces;
using TimeTracker.BLL.Models.Password;

namespace TimeTracker.BLL.Helpers;

public class PasswordHelper : IPasswordHelper
{
    public PasswordValidationResultModel ValidatePassword(string password)
    {
        var validationResult = new PasswordValidationResultModel();

        if (string.IsNullOrWhiteSpace(password))
        {
            return validationResult;
        }

        const string hasMinCountCharactersPattern = "^.{10,}$";
        const string hasOneUpperCaseLetterPattern = "(?=.*[A-Z])";
        const string hasOneLowerCaseLetterPattern = "(?=.*[a-z])";
        const string hasOneNumberRegex = @"(?=.*\d)";
        const string hasOneSpecialCharacterPattern = @"(?=.*[~!@#$%^&*_\-+=`|\(){}[\]:;""'<>,.?/])";

        validationResult.HasMinCountCharacters = new Regex(hasMinCountCharactersPattern).IsMatch(password);
        validationResult.HasOneUpperCaseLetter = new Regex(hasOneUpperCaseLetterPattern).IsMatch(password);
        validationResult.HasOneLowerCaseLetter = new Regex(hasOneLowerCaseLetterPattern).IsMatch(password);
        validationResult.HasOneNumber = new Regex(hasOneNumberRegex).IsMatch(password);
        validationResult.HasOneSpecialCharacter = new Regex(hasOneSpecialCharacterPattern).IsMatch(password);

        return validationResult;
    }
}