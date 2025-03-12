using System.ComponentModel;

using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению статуса книги.
/// </summary>
public sealed class UpdateBookStatusParams : UpdateBookParamsBase
{
    public BookStatus NewBookStatus { get; }

    public UpdateBookStatusParams(
        Id<Book> bookId,
        BookStatus newBookStatus)
        : base(bookId)
    {
        if (!Enum.IsDefined(newBookStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(newBookStatus),
                (int)newBookStatus,
                typeof(BookStatus));
        }

        NewBookStatus = newBookStatus;
    }
}
