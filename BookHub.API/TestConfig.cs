using System.ComponentModel.DataAnnotations;

namespace BookHub.API;

public class TestConfig
{
    [Required(ErrorMessage = "Can't be empty or has only whitespaces.")]
    public required string Content { get; init; }
}
