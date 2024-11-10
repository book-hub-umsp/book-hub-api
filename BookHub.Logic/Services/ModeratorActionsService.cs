using BookHub.Abstractions.Logic.Services;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Users;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services;

/// <summary>
/// Представляет сервис для обработки действий модератора.
/// </summary>
/// <remarks>
/// На каждый метод будет выполнять проверку наличия у пользователя модераторства
/// - и будем прокидывать ошибку в случае их отсутствия.
/// </remarks>
public sealed class ModeratorActionsService : IModeratorActionsService
{
    public ModeratorActionsService(
        IBooksHubUnitOfWork unitOfWork,
        ILogger<ModeratorActionsService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private async Task ValidateModeratorOptionsExistenseAsync(
        Id<User> userId, 
        CancellationToken token)
    {
        _logger.LogInformation(
            "Validating moderator options existense for user {UserId}", 
            userId.Value);

        var existense = await _unitOfWork.Users.HasModeratorOptions(userId, token);

        if (!existense) 
        { 
            _logger.LogInformation(
                "Moderator options not existed for user {UserId}", 
                userId.Value);

            throw new InvalidOperationException(
                $"Moderator options not existed for user {userId.Value}.");
        }
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger<ModeratorActionsService> _logger;
}