namespace BookHub.Storage.PostgreSQL.Models.Helpers;

/// <summary>
/// Вспомогательная модель превью книги из хранилища.
/// </summary>
public sealed class StorageBookPreviewHelper
{
    public required long BookId { get; init; }

    public required string Title { get; init; }

    public required BookGenre BookGenre { get; init; }

    public required long AuthorId { get; init; }

    public IEnumerable<StorageChapterHelper> Chapters { get; init; } = [];
}