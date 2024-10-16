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
            new("about"));
        var userStatus = UserStatus.Blocked;
        var userPremission = UserPermission.Moderation;

        // Act
        var user = new User(profileInfo, userStatus, userPremission);

        // Assert
        user.ProfileInfo.Should().Be(profileInfo);
        user.Status.Should().Be(userStatus);
        user.Permission.Should().Be(userPremission);
    }

    [Fact(DisplayName = "Cannot create without profile info.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProfileInfo()
    {
        // Act
        var exception = Record.Exception(() =>
            new User(
                null!,
                UserStatus.Blocked,
                UserPermission.Moderation));

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
                (UserStatus)int.MaxValue,
                UserPermission.Moderation));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
    }

    [Fact(DisplayName = "Cannot create when user premission is invalid.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenUserPremissionIsInvalid()
    {
        // Act
        var exception = Record.Exception(() =>
            new User(
                new UserProfileInfo(
                    new(123), 
                    new("name"), 
                    new("email@gmail.com"), 
                    new("about")),
                UserStatus.Blocked,
                (UserPermission)int.MaxValue));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
    }
}