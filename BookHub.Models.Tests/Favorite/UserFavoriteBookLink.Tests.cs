using BookHub.Models.Account;
using BookHub.Models.Books.Repository;
using BookHub.Models.Favorite;

using FluentAssertions;

using Xunit;

namespace BookHub.Models.Tests.Favorite;
public class UserFavoriteBookLinkTest
{
    [Fact(DisplayName = "Can create.")]
    [Trait("Category", "Unit")]
    public void CanCreate()
    {
        // Arrange
        var userId = new Id<User>(123);
        var bookId = new Id<Book>(123);

        // Act
        var useFavoriteBookLink = new UserFavoriteBookLink(userId, bookId);

        // Assert
        useFavoriteBookLink.UserId.Should().Be(userId);
        useFavoriteBookLink.BookId.Should().Be(bookId);
    }

    [Fact(DisplayName = "Cannot create without user id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutUserId()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserFavoriteBookLink(
                null!,
                new(123)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create without book id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutBookId()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserFavoriteBookLink(
                new(123),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
