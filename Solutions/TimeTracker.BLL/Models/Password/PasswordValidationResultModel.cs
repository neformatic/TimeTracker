namespace TimeTracker.BLL.Models.Password;

public class PasswordValidationResultModel
{
    public bool HasMinCountCharacters { get; set; }
    public bool HasOneUpperCaseLetter { get; set; }
    public bool HasOneLowerCaseLetter { get; set; }
    public bool HasOneNumber { get; set; }
    public bool HasOneSpecialCharacter { get; set; }

    public bool IsValid => HasMinCountCharacters &&
                           HasOneUpperCaseLetter &&
                           HasOneLowerCaseLetter &&
                           HasOneNumber &&
                           HasOneSpecialCharacter;
}