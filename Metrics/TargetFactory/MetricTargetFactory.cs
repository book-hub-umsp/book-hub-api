using BookHub.API.Metrics.Configurations;

using Microsoft.Extensions.Options;

namespace BookHub.API.Metrics.TargetFactory;

/// <summary>
/// Представляет фабрику для кастомных таргетов.
/// </summary>
public sealed class MetricTargetFactory :
    IMetricTargetFactory
{
    public MetricTargetFactory(
        IOptions<TargetAppSourceMetricsConfiguration> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _targets = [options.Value.AppSource];
    }


    public IEnumerable<string> CreateTargets() => _targets;

    private readonly IReadOnlyCollection<string> _targets;
}