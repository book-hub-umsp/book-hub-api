using BookHub.Models;
using BookHub.Models.Books.Content;
using BookHub.Models.Books.Repository;
using BookHub.Models.DomainEvents;

namespace BookHub.Abstractions.Logic.Services.Books.Content;

/// <summary>
/// Описывает сервис работы с главами книги.
/// </summary>
public interface IChaptersService
{
    /// <summary>
    /// Добавляет новую главу.
    /// </summary>
    /// <param name="creatingChapter">
    /// Модель создаваемой главы.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="creatingChapter"/> равен <see langword="null"/>.
    /// </exception>
    public Task AddChapterAsync(
        CreatingChapter creatingChapter, 
        CancellationToken token);

    /// <summary>
    /// Удаляет главу для заданной книги
    /// и пересчитывает номера остальных глав этой книги.
    /// </summary>
    /// <param name="chapterId">
    /// Идентификатор удаляемой главы.
    /// </param>
    /// <param name="bookId">
    /// Идентификатор связанной книги.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="chapterId"/> или <paramref name="bookId"/>
    /// равны <see langword="null"/>.
    /// </exception>
    public Task RemoveChapterAsync(
        Id<Chapter> chapterId, 
        Id<Book> bookId, 
        CancellationToken token);

    /// <summary>
    /// Получает контент главы по идентификатору.
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
    public Task<Chapter> GetChapterAsync(
        Id<Chapter> chapterId,
        CancellationToken token);

    /// <summary>
    /// Выполняет обновление контента главы книги.
    /// </summary>
    /// <param name="updatedChapter">
    /// Событие обновления контента главы.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="updatedChapter"/> равен <see langword="null"/>.
    /// </exception>
    public Task UpdateChapterAsync(
        UpdatedBase<Chapter> updatedChapter, 
        CancellationToken token);
}