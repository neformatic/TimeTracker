using TimeTracker.Common.Enums;

namespace TimeTracker.Common.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestMessageLevel MessageLevel { get; set; }

    public BadRequestException(string message, BadRequestMessageLevel level = BadRequestMessageLevel.Error) : base(message)
    {
        MessageLevel = level;
    }
}