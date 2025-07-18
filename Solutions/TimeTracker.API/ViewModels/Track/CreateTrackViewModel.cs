using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.ViewModels.Track;

public class CreateTrackViewModel
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    [MaxLength(255)]
    public string Artist { get; set; }

    [MaxLength(100)]
    public string Genre { get; set; }

    [Required]
    [MaxLength(1024)]
    public string FilePath { get; set; }
}