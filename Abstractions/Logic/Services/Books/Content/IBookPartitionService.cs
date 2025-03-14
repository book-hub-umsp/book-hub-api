using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Logic.Services.Books.Content;

/// <summary>
/// Описывает сервис работы с разделами книги.
/// </summary>
public interface IBookPartitionService
{
    /// <summary>
    /// Добавляет новый раздел.
    /// </summary>
    /// <param name="creatingPartition">
    /// Модель создаваемого раздела.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="creatingPartition"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователь не является автором книги, 
    /// в которую добавляет главу.
    /// </exception>
    public Task AddPartitionAsync(
        CreatingPartition creatingPartition,
        CancellationToken token);

    /// <summary>
    /// Удаляет главу для заданной книги
    /// и пересчитывает номера остальных глав этой книги.
    /// </summary>
    /// <param name="bookId">
    /// Идентификатор связанной книги.
    /// </param>
    /// <param name="partitionNumber">
    /// Номер раздела.
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
    public Task RemovePartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token);

    /// <summary>
    /// Получает контент раздела.
    /// </summary>
    /// <param name="bookId">
    /// Идентификатор книги.
    /// </param>
    /// <param name="partitionNumber">
    /// Номер раздела.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="bookId"/> равен <see langword="null"/>.
    /// </exception>
    public Task<Partition> GetPartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token);

    /// <summary>
    /// Выполняет обновление контента раздела книги.
    /// </summary>
    /// <param name="partitionUpdated">
    /// Событие обновления контента раздела.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="partitionUpdated"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователь не является автором книги, 
    /// в которой изменяет данный раздел.
    /// </exception>
    public Task UpdatePartitionAsync(
        PartitionUpdated<PartitionContent> partitionUpdated,
        CancellationToken token);
}