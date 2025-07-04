using System.Text.Json.Serialization;

namespace TimeTracker.BLL.Models.Templates;

public class ResetPasswordEmailTemplate
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}