using BookHub.API.Abstractions;
using BookHub.API.Abstractions.Logic.Services.Books.Content;
using BookHub.API.Abstractions.Storage;
using BookHub.API.Models;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;

using Microsoft.Extensions.Logging;

namespace BookHub.API.Logic.Services.Books.Content;

/// <summary>
/// Сервис по работе с главами книги.
/// </summary>
public sealed class ChaptersService : IChaptersService
{
    public ChaptersService(
        IBooksHubUnitOfWork unitOfWork,
        IUserIdentityFacade userIdentityFacade,
        ILogger<ChaptersService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userIdentityFacade = userIdentityFacade
            ?? throw new ArgumentNullException(nameof(userIdentityFacade));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddChapterAsync(
        CreatingChapter creatingChapter,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(creatingChapter);

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to add new chapter for book {BookId}" +
            " via user {UserId} request",
            creatingChapter.BookId.Value,
            currentUserId.Value);

        if (!await _unitOfWork.Books.IsUserAuthorForBook(
                currentUserId,
                creatingChapter.BookId,
                token))
        {
            throw new InvalidOperationException(
                $"Current user {currentUserId.Value} is not author" +
                $" for book {creatingChapter.BookId.Value}" +
                " and can not add chapters.");
        }

        await _unitOfWork.Chapters.AddChapterAsync(creatingChapter, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "New chapter for book {BookId} was added",
            creatingChapter.BookId.Value);
    }

    /// <inheritdoc/>
    public async Task RemoveChapterAsync(
        Id<Chapter> chapterId,
        Id<Book> bookId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);
        ArgumentNullException.ThrowIfNull(bookId);

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to remove chapter with sequence number {ChapterId}" +
            " for book {BookId}" +
            " via user {UserId} request",
            chapterId.Value,
            bookId.Value,
            currentUserId.Value);

        if (!await _unitOfWork.Books.IsUserAuthorForBook(
                currentUserId,
                bookId,
                token))
        {
            throw new InvalidOperationException(
                $"Current user {currentUserId.Value} is not author" +
                $" for book {bookId.Value}" +
                " and can not remove chapters.");
        }

        await _unitOfWork.Chapters.RemoveChapterAsync(chapterId, bookId, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Book's {BookId} chapters were changed" +
            " after removing chapter {ChapterId}",
            bookId.Value,
            chapterId.Value);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task UpdateChapterAsync(
        UpdatedChapter<ChapterContent> updatedChapter,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedChapter);

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to update content of chapter" +
            " with sequence number {ChapterId}" +
            " via user {UserId} request",
            updatedChapter.Id.Value,
            currentUserId.Value);

        if (!await _unitOfWork.Books.IsUserAuthorForBook(
            currentUserId,
            updatedChapter.BookId,
            token))
        {
            throw new InvalidOperationException(
                $"Current user {currentUserId.Value} is not author" +
                $" for book {updatedChapter.BookId.Value}" +
                " and can not update chapters.");
        }

        await _unitOfWork.Chapters.UpdateChapterAsync(updatedChapter, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Content of chapter with sequence number {ChapterId} was updated",
            updatedChapter.Id.Value);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly IUserIdentityFacade _userIdentityFacade;
    private readonly ILogger<ChaptersService> _logger;
}