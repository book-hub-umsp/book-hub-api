using BookHub.Abstractions.Storage;
using BookHub.Abstractions.Storage.Repositories;
using BookHub.Logic.Services.Account;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.DomainEvents;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace BookHub.Logic.Tests.Account;

public class UserServiceTests
{
    [Fact(DisplayName = "Can create.")]
    [Trait("Category", "Unit")]
    public void CanCreate()
    {
        // Act
        var exception = Record.Exception(() =>
            new UserService(
                Mock.Of<IBooksHubUnitOfWork>(MockBehavior.Strict),
                Mock.Of<ILogger<UserService>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can register new user if not exists.")]
    [Trait("Category", "Unit")]
    public async Task CanRegisterNewUserIfNotExistsAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var registeringUser = new RegisteringUser(
            new("somename@gmail.com"));

        var expectedProfileInfo = new UserProfileInfo(
            new(1),
            new("name"),
            registeringUser.Email,
            new("about"));

        var addCallback = 0;
        var userRepo = new Mock<IUsersRepository>(MockBehavior.Strict);
        userRepo.SetupSequence(x => x.FindUserProfileInfoByEmailAsync(registeringUser.Email, cts.Token))
            .ReturnsAsync((UserProfileInfo?)null)
            .ReturnsAsync(expectedProfileInfo);
        userRepo.Setup(x => x.AddUserAsync(registeringUser, cts.Token))
            .Returns(Task.CompletedTask)
            .Callback(() => addCallback++);

        var saveCallback = 0;
        var uow = new Mock<IBooksHubUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Users)
            .Returns(userRepo.Object);
        uow.Setup(x => x.SaveChangesAsync(cts.Token))
            .Returns(Task.CompletedTask)
            .Callback(() => saveCallback++);

        var service = new UserService(
            uow.Object,
            Mock.Of<ILogger<UserService>>());

        // Act
        var actual = await service.RegisterNewUserAsync(registeringUser, cts.Token);

        // Assert
        actual.Should().BeEquivalentTo(expectedProfileInfo);
        addCallback.Should().Be(1);
        saveCallback.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot register without registering user id.")]
    [Trait("Category", "Unit")]
    public async Task CanNotRegisterWithoutRegisteringUserAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var service = new UserService(
            Mock.Of<IBooksHubUnitOfWork>(MockBehavior.Strict),
            Mock.Of<ILogger<UserService>>());

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await service.RegisterNewUserAsync(null!, cts.Token));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can get user profile info.")]
    [Trait("Category", "Unit")]
    public async Task CanGetUserProfileInfoAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var id = new Id<User>(1);

        var expectedProfileInfo = new UserProfileInfo(
            new(1),
            new("name"),
            new("somename@gmail.com"),
            new("about"));

        var getCallback = 0;
        var userRepo = new Mock<IUsersRepository>(MockBehavior.Strict);
        userRepo.Setup(x => x.GetUserProfileInfoByIdAsync(id, cts.Token))
            .ReturnsAsync(expectedProfileInfo)
            .Callback(() => getCallback++);

        var uow = new Mock<IBooksHubUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Users)
            .Returns(userRepo.Object);

        var service = new UserService(
            uow.Object,
            Mock.Of<ILogger<UserService>>());

        // Act
        var actual = await service.GetUserProfileInfoAsync(id, cts.Token);

        // Assert
        actual.Should().BeEquivalentTo(expectedProfileInfo);
        getCallback.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot get user profile info without id.")]
    [Trait("Category", "Unit")]
    public async Task CanNotGetUserProfileInfoWithoutIdAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var service = new UserService(
            Mock.Of<IBooksHubUnitOfWork>(MockBehavior.Strict),
            Mock.Of<ILogger<UserService>>());

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await service.GetUserProfileInfoAsync(null!, cts.Token));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can update user profile info.")]
    [Trait("Category", "Unit")]
    public async Task CanUpdateUserProfileInfoAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var updated = new FakeUpdated(new(1));

        var updateCallback = 0;
        var userRepo = new Mock<IUsersRepository>(MockBehavior.Strict);
        userRepo.Setup(x => x.UpdateUserAsync(updated, cts.Token))
            .Returns(Task.CompletedTask)
            .Callback(() => updateCallback++);

        var saveCallback = 0;
        var uow = new Mock<IBooksHubUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Users)
            .Returns(userRepo.Object);
        uow.Setup(x => x.SaveChangesAsync(cts.Token))
            .Returns(Task.CompletedTask)
            .Callback(() => saveCallback++);

        var service = new UserService(
            uow.Object,
            Mock.Of<ILogger<UserService>>());

        // Act
        await service.UpdateUserInfoAsync(updated, cts.Token);

        // Assert
        updateCallback.Should().Be(1);
        saveCallback.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot update user profile info without update.")]
    [Trait("Category", "Unit")]
    public async Task CanNotUpdateUserInfoWithoutUpdateAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var service = new UserService(
            Mock.Of<IBooksHubUnitOfWork>(MockBehavior.Strict),
            Mock.Of<ILogger<UserService>>());

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await service.UpdateUserInfoAsync(null!, cts.Token));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    public sealed class FakeUpdated : UpdatedBase<User>
    {
        public FakeUpdated(Id<User> id) : base(id)
        {
        }
    }
}