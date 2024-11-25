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
    /// <exception cref="InvalidOperationException">
    /// Если в книге уже есть максимальное число глав.
    /// Или если номер добавляемой главы не соответствует ожидаемому.
    /// </exception>
    public async Task AddChapterAsync(
        CreatingChapter chapter,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapter);

        var existedBook = await Context.Books
            .Include(x => x.Chapters)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == chapter.BookId.Value, token) 
            ?? throw new InvalidOperationException(
                $"No related book {chapter.BookId.Value} for creating chapter.");

        var currentBookChapters = existedBook.Chapters.ToList();

        if (currentBookChapters.Count == ChapterNumber.MAX_NUMBER)
        {
            throw new InvalidOperationException(
                $"Book {chapter.BookId.Value} already" +
                $" has max amount of chapters - {ChapterNumber.MAX_NUMBER}.");
        }

        var expectedChapterNumber = currentBookChapters.Count + 1;

        if (chapter.ChapterNumber.Value != expectedChapterNumber)
        {
            throw new InvalidOperationException(
                $"New chapter for book {chapter.BookId.Value} " +
                $"should has number {expectedChapterNumber}.");
        }

        var storageChapter = new StorageChapter
        {
            Number = chapter.ChapterNumber.Value,
            Content = chapter.Content.Value,
            BookId = chapter.BookId.Value
        };

        Context.Chapters.Add(storageChapter);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// При удалении главы из книги делается перерасчет номеров остальных глав.
    /// </remarks>
    public async Task RemoveChapterAsync(
        Id<Chapter> chapterId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);

        var existedChapter = await Context.Chapters
            .FindAsync([chapterId.Value], token) 
            ?? throw new InvalidOperationException(
                $"Chapter {chapterId.Value} not found");

        Context.Chapters.Remove(existedChapter);

        var restOfChapters = 
            await Context.Chapters
                .Where(x => x.BookId == existedChapter.BookId).ToListAsync(token);

        foreach (var chapter in restOfChapters)
        {
            if (chapter.Number > existedChapter.Number)
            {
                chapter.Number -= 1;
            }
        }
    }

    /// <inheritdoc/>
    public async Task<Chapter> GetChapterByIdAsync(
        Id<Chapter> chapterId, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(chapterId);

        var existedChapter = await Context.Chapters
            .FindAsync([chapterId.Value], token);

        return existedChapter is null
            ? throw new InvalidOperationException(
                $"Chapter {chapterId.Value} not found")
            : new(
                chapterId,
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
            .FindAsync([updatedChapter.Id.Value], token) 
            ?? throw new InvalidOperationException(
                $"Chapter {updatedChapter.Id.Value} not found");

        switch (updatedChapter)
        {
            case UpdatedChapter<ChapterContent> contentUpdate:

                existedChapter.Content = contentUpdate.Attribute.Value;
                break;

            default:
                throw new InvalidOperationException(
                    $"Update type: {updatedChapter.GetType().Name} is not supported.");
        }
    }
}