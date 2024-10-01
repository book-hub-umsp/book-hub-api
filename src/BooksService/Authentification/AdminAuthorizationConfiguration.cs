namespace BooksService.Authentification;

/// <summary>
/// Конфигураци, содержащая список администраторов сервиса.
/// </summary>
public sealed class AdminAuthorizationConfiguration
{
    /// <summary>
    /// Список авторизованных клиентов.
    /// </summary>
    public IReadOnlyCollection<string> Admins { get; set; } = [];
}
