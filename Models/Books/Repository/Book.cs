using System.ComponentModel;

using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.Books.Repository;

/// <summary>
/// Модель книги.
/// </summary>
public sealed class Book
{
    public Id<Book> Id { get; }

    public Id<User> AuthorId { get; }

    public BookDescription Description { get; private set; }

    public BookStatus Status { get; private set; }

    public DateTimeOffset CreationDate { get; }

    public DateTimeOffset LastEditDate { get; private set; }

    public Book(
        Id<Book> id,
        Id<User> authorId,
        BookDescription description,
        BookStatus status = BookStatus.Published)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));

        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));

        Description = description
            ?? throw new ArgumentNullException(nameof(description));

        if (!Enum.IsDefined(status))
        {
            throw new InvalidEnumArgumentException(
                nameof(status),
                (int)status,
                typeof(BookStatus));
        }

        Status = status;

        CreationDate = DateTimeOffset.UtcNow;
        LastEditDate = CreationDate;
    }

    public void ChangeDescription(BookDescription newDescription)
    {
        ArgumentNullException.ThrowIfNull(newDescription);

        if (Description == newDescription)
        {
            return;
        }

        Description = newDescription;
        LastEditDate = DateTimeOffset.UtcNow;
    }
}