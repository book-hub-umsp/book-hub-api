using BookHub.API.Abstractions;
using BookHub.API.Abstractions.Logic.Services.Books.Content;
using BookHub.API.Abstractions.Storage;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

using Microsoft.Extensions.Logging;

namespace BookHub.API.Logic.Services.Books.Content;

/// <summary>
/// Сервис по работе с главами книги.
/// </summary>
public sealed class BookPartitionService : IBookPartitionService
{
    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly IUserIdentityFacade _userIdentityFacade;
    private readonly ILogger<BookPartitionService> _logger;

    public BookPartitionService(
        IBooksHubUnitOfWork unitOfWork,
        IUserIdentityFacade userIdentityFacade,
        ILogger<BookPartitionService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userIdentityFacade = userIdentityFacade
            ?? throw new ArgumentNullException(nameof(userIdentityFacade));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddPartitionAsync(
        CreatingPartition creatingChapter,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(creatingChapter);

        await CheckAccessAsync(creatingChapter.BookId, token);

        await _unitOfWork.Chapters.AddPartitionAsync(creatingChapter, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "New partition for book {BookId} was added",
            creatingChapter.BookId.Value);
    }

    /// <inheritdoc/>
    public async Task RemovePartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(partitionNumber);

        await CheckAccessAsync(bookId, token);

        await _unitOfWork.Chapters.RemovePartitionAsync(bookId, partitionNumber, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Book's {BookId} partitions were changed" +
            " after removing partition {ChapterId}",
            bookId.Value,
            partitionNumber.Value);
    }

    /// <inheritdoc/>
    public async Task<Partition> GetPartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(partitionNumber);

        var chapter = await _unitOfWork.Chapters
            .GetPartitionAsync(bookId, partitionNumber, token);

        return chapter;
    }

    /// <inheritdoc/>
    public async Task UpdatePartitionAsync(
        PartitionUpdated<PartitionContent> updatedChapter,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedChapter);

        var (bookId, partitionNumber) = updatedChapter.Id;

        await CheckAccessAsync(bookId, token);

        await _unitOfWork.Chapters.UpdatePartitionAsync(updatedChapter, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Content of partition {PartitionId} was updated",
            partitionNumber.Value);
    }

    private async Task CheckAccessAsync(Id<Book> bookId, CancellationToken token)
    {
        var currentUserId = _userIdentityFacade.Id!;

        var isCurrentUserAuthor = await _unitOfWork.Books
            .IsUserAuthorForBook(currentUserId, bookId, token);

        if (!isCurrentUserAuthor)
        {
            throw new InvalidOperationException(
                $"Current user {currentUserId.Value} is not author" +
                $" for book {bookId.Value}.");
        }
    }
}