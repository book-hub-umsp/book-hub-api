using System.ComponentModel.DataAnnotations;

namespace BookHub.Storage.Models;

/// <summary>
/// Конфигурация лимита книги.
/// </summary>
public sealed class BookTextConfiguration
{
    [Required]
    [Range(
        LEFT_BOUND_MAX_LIMIT, 
        RIGHT_BOUND_MAX_LIMIT, 
        ErrorMessage = "Book symbols max limit must be in range of limit bounds.")]
    public required int BookSymbolsMaxLimit { get; init; }

    private const int LEFT_BOUND_MAX_LIMIT = 1_000;
    private const int RIGHT_BOUND_MAX_LIMIT = int.MaxValue;
}