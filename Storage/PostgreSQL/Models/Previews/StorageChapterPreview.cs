namespace BookHub.API.Storage.PostgreSQL.Models.Previews;

/// <summary>
/// Вспомогательная модель главы.
/// </summary>
public sealed class StorageChapterPreview
{
    public required long ChapterId { get; init; }

    public required long BookId { get; init; }

    public required int SequenceNumber { get; init; }
}