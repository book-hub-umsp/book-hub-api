using BookHub.Models.Books;
using BookHub.Models.Users;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры для получения и удаления избранного
/// </summary>
public sealed class FavoriteLinkParams
{
    public Id<Book>  BookId { get; }

    public Id<User> UserId { get; }

    public FavoriteLinkParams(Id<Book> bookId, Id<User> Userid)
    {
        BookId = bookId;
        UserId = Userid;
    }
}
