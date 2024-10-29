using BookHub.Models.Users;
using FluentAssertions;
using Xunit;

namespace BookHub.Models.Tests;
public class UsersFavoriteTests
{
    [Fact(DisplayName = "Can create.")]
    [Trait("Category", "Unit")]
    public void CanCreate()
    {
        // Arrange
        var userId = new Id<User>(123);
        var links = new HashSet<UserFavoriteBookLink>() { new(new(123), new(321)) };

        // Act
        var useFavoriteBookLink = new UsersFavorite(userId, links);

        // Assert
        useFavoriteBookLink.UserId.Should().Be(userId);
        useFavoriteBookLink.Links.Should().BeEquivalentTo(links);
    }

    [Fact(DisplayName = "Cannot create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutUserId()
    {
        // Arrange
        var links = new HashSet<UserFavoriteBookLink>() { new(new(123), new(321)) };

        // Act
        var exception = Record.Exception(() =>
            new UsersFavorite(
                null!,
                links));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create without name.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutLinks()
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
