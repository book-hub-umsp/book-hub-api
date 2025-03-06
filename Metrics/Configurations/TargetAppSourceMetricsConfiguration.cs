using System.ComponentModel.DataAnnotations;

namespace BookHub.Metrics.Configurations;

/// <summary>
/// Конфигурация указания таргета источника приложения.
/// </summary>
public sealed class TargetAppSourceMetricsConfiguration
{
    [Required(ErrorMessage = "Can't be null, empty or has only whitespaces.")]
    public required string AppSource { get; init; }
}