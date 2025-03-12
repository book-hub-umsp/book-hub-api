using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace BookHub.API.Metrics;

/// <summary>
/// Инструментация для работы с метриками.
/// </summary>
public sealed class ActivityToMetricsInstrumentation : IDisposable
{
    public ActivityToMetricsInstrumentation(
        string target)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(target);

        _meter = new Meter(target);

        _listener = new ActivityListener
        {
            Sample =
            (ref ActivityCreationOptions<ActivityContext> _)
                => ActivitySamplingResult.PropagationData,

            ShouldListenTo = (source) =>
                source.Name.Equals(target, StringComparison.Ordinal),

            ActivityStopped = (activity) =>
            {
                var histogram = _histogramCache.GetOrAdd(
                    activity.DisplayName,
                    (name) => _meter.CreateHistogram<double>(name, "ms"));

                histogram.Record(
                    activity.Duration.TotalMilliseconds,
                    activity.TagObjects.ToArray());
            }
        };

        ActivitySource.AddActivityListener(_listener);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _meter.Dispose();
            _listener.Dispose();

            _disposed = true;
        }
    }

    private readonly ConcurrentDictionary<string, Histogram<double>> _histogramCache = new();
    private readonly Meter _meter;
    private readonly ActivityListener _listener;
    private bool _disposed;
}