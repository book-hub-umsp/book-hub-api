using BookHub.Abstractions.Logic.Services.Books.Content;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Books.Content;
using BookHub.Models.Books.Repository;
using BookHub.Models.DomainEvents;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services.Books.Content;

/// <summary>
/// Сервис по работе с главами книги.
/// </summary>
public sealed class ChaptersService : IChaptersService
{
    public ChaptersService(
        IBooksHubUnitOfWork unitOfWork,
        ILogger<ChaptersService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddChapterAsync(
        CreatingChapter creatingChapter, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(creatingChapter);

        _logger.LogDebug(
            "Trying to add new chapter for book {BookId}", 
            creatingChapter.BookId.Value);

        await _unitOfWork.Chapters.AddChapterAsync(creatingChapter, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "New chapter for book {BookId} was added",
            creatingChapter.BookId.Value);
    }

    public async Task RemoveChapterAsync(
        Id<Chapter> chapterId,
        Id<Book> bookId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);
        ArgumentNullException.ThrowIfNull(bookId);

        _logger.LogDebug(
            "Trying to remove chapter with sequence number {ChapterId} for book {BookId}",
            chapterId.Value,
            bookId.Value);

        await _unitOfWork.Chapters.RemoveChapterAsync(chapterId, bookId, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Book's {BookId} chapters were changed" +
            " after removing chapter {ChapterId}",
            bookId.Value,
            chapterId.Value);
    }

    public async Task<Chapter> GetChapterAsync(
        Id<Chapter> chapterId, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);

        _logger.LogDebug(
            "Trying to get content of chapter" +
            " with sequence number {ChapterId}",
            chapterId.Value);

        var chapter = await _unitOfWork.Chapters.GetChapterByIdAsync(chapterId, token);

        _logger.LogInformation(
            "Chapter with sequence number {ChapterId} for book {BookId} was received",
            chapterId.Value,
            chapter.BookId.Value);

        return chapter;
    }

    public async Task UpdateChapterAsync(
        UpdatedBase<Chapter> updatedChapter, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedChapter);

        _logger.LogDebug(
            "Trying to update content of chapter" +
            " with sequence number {ChapterId}",
            updatedChapter.Id.Value);

        await _unitOfWork.Chapters.UpdateChapterAsync(updatedChapter, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Content of chapter with sequence number {ChapterId} was updated",
            updatedChapter.Id.Value);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger<ChaptersService> _logger;
}