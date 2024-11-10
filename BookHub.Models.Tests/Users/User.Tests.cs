using BookHub.Models.Users;
using FluentAssertions;
using System.ComponentModel;
using Xunit;

namespace BookHub.Models.Tests.Users;

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
            new("about"),
            UserRole.Default);
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

    [Fact(DisplayName = "Cannot create when user status is invalid.")]
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
                    new("about"),
                    UserRole.Default),
                (UserStatus)int.MaxValue));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
    }
}