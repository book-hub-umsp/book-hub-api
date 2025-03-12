using System.ComponentModel.DataAnnotations;

namespace BookHub.API.Metrics.Configurations;

/// <summary>
/// Конфигурация сервиса метрик Kestrel.
/// </summary>
public sealed class KestrelMetricServerConfiguration
{
    [Required(ErrorMessage = "Can't be null, empty or has only whitespaces.")]
    public required string Host { get; init; }

    public required int Port { get; init; }
}