namespace TimeTracker.BLL.Helpers.Interfaces;

public interface IHashHelper
{
    string GenerateHash(string password);
    bool VerifyHash(string hashedPassword, string password);
}