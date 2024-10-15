using BookHub.Models.Users;
using FluentAssertions;
using Xunit;

namespace BookHub.Models.Tests;

public class UserProfileInfoTests
{
    [Fact(DisplayName = "Can create.")]
    [Trait("Category", "Unit")]
    public void CanCreate()
    {
        // Arrange
        var name = new Name<User>("name");
        var email = new Email("email@gmail.com");
        var about = new About("some");

        // Act
        var profileInfo = new UserProfileInfo(name, email, about);

        // Assert
        profileInfo.Name.Should().Be(name);
        profileInfo.Email.Should().Be(email);
        profileInfo.About.Should().Be(about);
    }

    [Fact(DisplayName = "Cannot create without name.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutName()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserProfileInfo(
                null!,
                new("email@gmail.com"),
                new("about")));

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
                new("name"),
                null!,
                new("about")));

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
                new("name"),
                new("email@gmail.com"),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}