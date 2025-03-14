using System.ComponentModel;

using BookHub.API.Models.Account;

using FluentAssertions;

using Xunit;

namespace BookHub.API.Models.Tests.Account;

public class UserTests
{
    [Fact(DisplayName = "Can create.")]
    [Trait("Category", "Unit")]
    public void CanCreate()
    {
        // Arrange
        var profileInfo = new UserProfileInfo(
            new(123),
            new("name"),
            new("email@gmail.com"),
            new("about"));
        var userStatus = UserStatus.Blocked;

        // Act
        var user = new User(profileInfo, userStatus);

        // Assert
        user.ProfileInfo.Should().Be(profileInfo);
        user.Status.Should().Be(userStatus);
    }

    [Fact(DisplayName = "Cannot create without profile info.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProfileInfo()
    {
        // Act
        var exception = Record.Exception(() =>
            new User(
                null!,
                UserStatus.Blocked));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create when user premission is invalid.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenUserStatusIsInvalid()
    {
        // Act
        var exception = Record.Exception(() =>
            new User(
                new UserProfileInfo(
                    new(123),
                    new("name"),
                    new("email@gmail.com"),
                    new("about")),
                (UserStatus)int.MaxValue));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
    }
}