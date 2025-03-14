namespace BookHub.API.Metrics.TargetFactory;

/// <summary>
/// Описывает фабрику для кастомных таргетов.
/// </summary>
public interface IMetricTargetFactory
{
    IEnumerable<string> CreateTargets();
}