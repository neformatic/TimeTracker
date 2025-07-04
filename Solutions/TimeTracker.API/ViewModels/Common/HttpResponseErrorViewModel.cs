using TimeTracker.Common.Enums;

namespace TimeTracker.API.ViewModels.Common;

public class HttpResponseErrorViewModel
{
    public string ErrorMessage { get; set; }
    public BadRequestMessageLevel ErrorMessageLevel { get; set; }
}