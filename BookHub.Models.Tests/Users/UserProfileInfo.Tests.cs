using BookHub.Models.Users;
using FluentAssertions;
using System.Net.Mail;
using Xunit;

namespace BookHub.Models.Tests.Users;

public class UserProfileInfoTests
{
    [Fact(DisplayName = "Can create.")]
    [Trait("Category", "Unit")]
    public void CanCreate()
    {
        // Arrange
        var id = new Id<User>(123);
        var name = new Name<User>("name");
        var email = new MailAddress("email@gmail.com");
        var about = new About("some");
        var userRole = UserRole.Moderator;

        // Act
        var profileInfo = new UserProfileInfo(id, name, email, about, userRole);

        // Assert
        profileInfo.Id.Should().Be(id);
        profileInfo.Name.Should().Be(name);
        profileInfo.Email.Should().Be(email);
        profileInfo.About.Should().Be(about);
        profileInfo.Role.Should().Be(userRole);
    }

    [Fact(DisplayName = "Cannot create without id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutId()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserProfileInfo(
                null!,
                new("name"),
                new("email@gmail.com"),
                new("about"),
                UserRole.Moderator));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create without name.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutName()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserProfileInfo(
                new(123),
                null!,
                new("email@gmail.com"),
                new("about"),
                UserRole.Moderator));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create without email.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutEmail()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserProfileInfo(
                new(123),
                new("name"),
                null!,
                new("about"),
                UserRole.Moderator));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create without about.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutAbout()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserProfileInfo(
                new(123),
                new("name"),
                new("email@gmail.com"),
                null!,
                UserRole.Moderator));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot create with invalid user role.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoInvalidRole()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserProfileInfo(
                new(123),
                new("name"),
                new("email@gmail.com"),
                new("about"),
                (UserRole)int.MaxValue));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}