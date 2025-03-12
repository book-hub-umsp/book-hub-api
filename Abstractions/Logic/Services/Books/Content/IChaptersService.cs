using BookHub.API.Models;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;

namespace BookHub.API.Abstractions.Logic.Services.Books.Content;

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
    /// <exception cref="InvalidOperationException">
    /// Если пользователь не является автором книги, 
    /// в которую добавляет главу.
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
    /// <exception cref="InvalidOperationException">
    /// Если пользователь не является автором книги, 
    /// из которой удаляет главу.
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
    /// <exception cref="InvalidOperationException">
    /// Если пользователь не является автором книги, 
    /// в которой изменяет данную главу.
    /// </exception>
    public Task UpdateChapterAsync(
        UpdatedChapter<ChapterContent> updatedChapter,
        CancellationToken token);
}