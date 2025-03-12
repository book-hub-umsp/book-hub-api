using BookHub.API.Models;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий глав книги.
/// </summary>
public interface IChaptersRepository
{
    /// <summary>
    /// Добавляет новую главу для книги.
    /// </summary>
    /// <param name="chapter">
    /// Модель создаваемой главы.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="chapter"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если связанной книги не существует.
    /// </exception>
    public Task AddChapterAsync(CreatingChapter chapter, CancellationToken token);

    /// <summary>
    /// Удаляет главу по идентификатору.
    /// </summary>
    /// <param name="chapterId">
    /// Идентификатор главы.
    /// </param>
    /// <param name="bookId">
    /// Идентификатор книги.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="chapterId"/> или <paramref name="bookId"/> 
    /// равны <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует главы с идентификатором <paramref name="chapterId"/>.
    /// </exception>
    public Task RemoveChapterAsync(
        Id<Chapter> chapterId,
        Id<Book> bookId,
        CancellationToken token);

    /// <summary>
    /// Получает главу по идентификатору.
    /// </summary>
    /// <param name="chapterId">
    /// Идентификатор главы.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="chapterId"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует главы с идентификатором <paramref name="chapterId"/>.
    /// </exception>
    public Task<Chapter> GetChapterByIdAsync(Id<Chapter> chapterId, CancellationToken token);

    /// <summary>
    /// Обновляет содержимое главы.
    /// </summary>
    /// <param name="updatedChapter">
    /// Модель обновления главы.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="updatedChapter"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует указанной в модели главы.
    /// </exception>
    public Task UpdateChapterAsync(
        UpdatedChapter<ChapterContent> updatedChapter,
        CancellationToken token);
}