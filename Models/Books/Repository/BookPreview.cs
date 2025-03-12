using BookHub.API.Models.Account;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.Books.Repository;

/// <summary>
/// Короткое описание книги.
/// </summary>
public sealed class BookPreview
{
    public Id<Book> Id { get; }

    public Name<Book> Title { get; }

    public BookGenre Genre { get; }

    public Id<User> AuthorId { get; }

    public IReadOnlyCollection<PartitionSequenceNumber> Partitions { get; }

    public BookPreview(
        Id<Book> id,
        Name<Book> title,
        BookGenre genre,
        Id<User> authorId,
        IReadOnlyCollection<PartitionSequenceNumber> partitions)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
        Partitions = partitions
            ?? throw new ArgumentNullException(nameof(partitions));
    }
}