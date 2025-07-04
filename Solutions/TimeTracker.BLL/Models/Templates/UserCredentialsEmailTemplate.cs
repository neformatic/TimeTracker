using System.Text.Json.Serialization;

namespace TimeTracker.BLL.Models.Templates;

public class UserCredentialsEmailTemplate
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}