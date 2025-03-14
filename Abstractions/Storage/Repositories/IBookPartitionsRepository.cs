using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий разделов книги.
/// </summary>
public interface IBookPartitionsRepository
{
    /// <summary>
    /// Добавляет новый раздел для книги.
    /// </summary>
    /// <param name="partition">
    /// Модель создаваемого раздела.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="partition"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если связанной книги не существует.
    /// </exception>
    public Task AddPartitionAsync(CreatingPartition partition, CancellationToken token);

    /// <summary>
    /// Удаляет раздел книги.
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
    /// Если <paramref name="bookId"/> или <paramref name="partitionNumber"/> 
    /// равны <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует такого раздела.
    /// </exception>
    /// <remarks>
    /// При удалении главы из книги делается перерасчет номеров остальных глав.
    /// </remarks>
    public Task RemovePartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token);

    /// <summary>
    /// Получает раздел книги.
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
    /// Если <paramref name="chapterId"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует такого раздела.
    /// </exception>
    public Task<Partition> GetPartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token);

    /// <summary>
    /// Обновляет содержимое раздела.
    /// </summary>
    /// <param name="updatedPartition">
    /// Модель обновления раздела.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="updatedPartition"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если раздела не существует.
    /// </exception>
    public Task UpdatePartitionAsync(
        PartitionUpdated<PartitionContent> updatedPartition,
        CancellationToken token);
}