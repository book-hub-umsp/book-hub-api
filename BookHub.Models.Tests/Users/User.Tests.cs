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
        var userRole = UserRole.Admin;
        var userStatus = UserStatus.Blocked;

        // Act
        var user = new User(profileInfo, userRole, userStatus);

        // Assert
        user.ProfileInfo.Should().Be(profileInfo);
        user.Status.Should().Be(userStatus);
        user.Role.Should().Be(userRole);
    }

    [Fact(DisplayName = "Cannot create without profile info.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProfileInfo()
    {
        // Act
        var exception = Record.Exception(() =>
            new User(
                null!,
                UserRole.Admin,
                UserStatus.Blocked));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create when user role is invalid.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenUserRoleIsInvalid()
    {
        // Act
        var exception = Record.Exception(() =>
            new User(
                new UserProfileInfo(
                    new(123),
                    new("name"),
                    new("email@gmail.com"),
                    new("about")),
                (UserRole)int.MaxValue,
                UserStatus.Active));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
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
                    new("about")),
                UserRole.Admin,
                (UserStatus)int.MaxValue));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
    }
}