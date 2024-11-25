using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.Books.Content;
using BookHub.Models.DomainEvents;
using BookHub.Models.DomainEvents.Books;
using BookHub.Storage.PostgreSQL.Abstractions;


using Microsoft.EntityFrameworkCore;

using StorageChapter = BookHub.Storage.PostgreSQL.Models.Chapter;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Представляет репозиторий глав.
/// </summary>
public sealed class ChaptersRepository : 
    RepositoryBase, 
    IChaptersRepository
{
    public ChaptersRepository(IRepositoryContext context)
        : base(context) { }

    /// <inheritdoc/>
    public async Task AddChapterAsync(
        CreatingChapter chapter,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapter);

        if (await Context.Books
            .SingleOrDefaultAsync(x => x.Id == chapter.BookId.Value, token) is null)
        {
            throw new InvalidOperationException(
                $"No related book {chapter.BookId.Value} for creating chapter.");
        }

        var storageChapter = new StorageChapter
        {
            Number = chapter.ChapterNumber.Value,
            Title = chapter.Title.Value,
            Content = chapter.Content.Value,
            BookId = chapter.BookId.Value
        };

        Context.Chapters.Add(storageChapter);
    }

    /// <inheritdoc/>
    public async Task RemoveChapterAsync(
        Id<Chapter> chapterId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);

        var existedChapter = await Context.Chapters
            .SingleOrDefaultAsync(x => x.Id == chapterId.Value, token);

        if (existedChapter is null)
        {
            throw new InvalidOperationException($"Chapter {chapterId.Value} not found");
        }

        Context.Chapters.Remove(existedChapter);
    }

    /// <inheritdoc/>
    public async Task<Chapter> GetChapterByIdAsync(
        Id<Chapter> chapterId, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);

        var existedChapter = await Context.Chapters
            .SingleOrDefaultAsync(x => x.Id == chapterId.Value, token);

        if (existedChapter is null)
        {
            throw new InvalidOperationException($"Chapter {chapterId.Value} not found");
        }

        return new(
            chapterId,
            new(existedChapter.Title),
            new(existedChapter.Number),
            new(existedChapter.BookId),
            new(existedChapter.Content));
    }

    /// <inheritdoc/>
    public async Task UpdateChapterAsync(
        UpdatedBase<Chapter> updatedChapter, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedChapter);

        var existedChapter = await Context.Chapters
            .SingleOrDefaultAsync(x => x.Id == updatedChapter.Id.Value, token);

        if (existedChapter is null)
        {
            throw new InvalidOperationException($"Chapter {updatedChapter.Id.Value} not found");
        }

        switch (updatedChapter)
        {
            case UpdatedChapter<Name<Chapter>> titleUpdate:

                existedChapter.Title = titleUpdate.Attribute.Value;

                break;

            case UpdatedChapter<ChapterNumber> numberUpdate:

                existedChapter.Number = numberUpdate.Attribute.Value;

                break;

            case UpdatedChapter<ChapterContent> contentUpdate:

                existedChapter.Content = contentUpdate.Attribute.Value;
                break;

            default:
                throw new InvalidOperationException(
                    $"Update type: {updatedChapter.GetType().Name} is not supported.");
        }
    }
}