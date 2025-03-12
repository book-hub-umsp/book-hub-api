using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;
using BookHub.API.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL.Repositories;

/// <summary>
/// Представляет репозиторий глав.
/// </summary>
public sealed class BookPartitionsRepository :
    RepositoryBase,
    IBookPartitionsRepository
{
    public BookPartitionsRepository(IRepositoryContext context)
        : base(context) { }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// Если в книге уже есть максимальное число глав.
    /// Или если номер добавляемой главы не соответствует ожидаемому.
    /// </exception>
    public async Task AddPartitionAsync(
        CreatingPartition partition,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(partition);

        var existedBook = await Context.Books
            .AsNoTracking()
            .GroupJoin(
                Context.Partitions,
                book => book.Id,
                partition => partition.BookId,
                (book, partitions) => new { book.Id, PartitionCount = partitions.Count() })
            .SingleOrDefaultAsync(x => x.Id == partition.BookId.Value, token)
            ?? throw new InvalidOperationException(
                $"No related book {partition.BookId.Value} for creating partition.");

        if (existedBook.PartitionCount == PartitionSequenceNumber.MAX_NUMBER)
        {
            throw new InvalidOperationException(
                $"Book {partition.BookId.Value} already" +
                $" has max amount of partitions - {PartitionSequenceNumber.MAX_NUMBER}.");
        }

        Context.Partitions.Add(new()
        {
            SequenceNumber = existedBook.PartitionCount + 1,
            Content = partition.Content.Value,
            BookId = partition.BookId.Value
        });
    }

    /// <inheritdoc/>
    public async Task RemovePartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(partitionNumber);

        var bookPartitions = await Context.Partitions
            .Where(x => x.BookId == bookId.Value)
            .ToListAsync(token);

        var chapterToRemove = await Context.Partitions
            .FindAsync([bookId.Value, partitionNumber.Value], token)
            ?? throw new InvalidOperationException(
                $"Partition {partitionNumber.Value} not found.");

        Context.Partitions.Remove(chapterToRemove);

        foreach (var partition in bookPartitions)
        {
            if (partition.SequenceNumber > chapterToRemove.SequenceNumber)
            {
                partition.SequenceNumber -= 1;
            }
        }
    }

    /// <inheritdoc/>
    public async Task<Partition> GetPartitionAsync(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);

        var existedPartition = await Context.Partitions
            .FindAsync([bookId.Value, partitionNumber.Value], token);

        return existedPartition is null
            ? throw new InvalidOperationException(
                $"Partition {partitionNumber.Value} for book {bookId.Value} not found.")
            : new(
                bookId, 
                new(existedPartition.SequenceNumber), 
                new(existedPartition.Content));
    }

    /// <inheritdoc/>
    public async Task UpdatePartitionAsync(
        PartitionUpdated<PartitionContent> partitionUpdated,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(partitionUpdated);

        var (bookId, partitionNumber) = partitionUpdated.Id;

        var existedPartition = await Context.Partitions
            .FindAsync([bookId.Value, partitionNumber.Value], token)
            ?? throw new InvalidOperationException(
                $"Partition {partitionNumber.Value} for book {bookId.Value} not found.");

        existedPartition.Content = partitionUpdated.Attribute.Value;
    }
}