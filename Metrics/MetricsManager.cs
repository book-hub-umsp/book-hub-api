using System.Diagnostics;
using System.Diagnostics.Metrics;

using BookHub.Abstractions.Metrics;
using BookHub.Metrics.Configurations;

using Microsoft.Extensions.Options;

namespace BookHub.Metrics;

public sealed class MetricsManager : IMetricsManager
{
    public MetricsManager(
        IOptions<TargetAppSourceMetricsConfiguration> options)
    {
        var appSource = options?.Value.AppSource
            ?? throw new ArgumentNullException(nameof(options));

        _meter = new Meter(appSource);
        _activitySource = new ActivitySource(appSource);
        _counter = _meter.CreateCounter<long>(ACTIVITY_NAME_COUNTING);
    }

    public IDisposable? MeasureRequestsExecution(string apiMethodName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiMethodName);

        ObjectDisposedException.ThrowIf(_disposed, this);

        return _activitySource
            .StartActivity(ACTIVITY_NAME_MEASURING)
            ?.SetTag(TAG_KEY, apiMethodName);
    }

    public void CountRequestsCalls(string apiMethodName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiMethodName);

        _counter.Add(1, new KeyValuePair<string, object?>(TAG_KEY, apiMethodName));
    }

    private readonly Meter _meter;
    private readonly ActivitySource _activitySource;
    private bool _disposed;

    private readonly Counter<long> _counter;

    private const string ACTIVITY_NAME_MEASURING = "execute_request";
    private const string ACTIVITY_NAME_COUNTING = "request_calls_count";

    private const string TAG_KEY = "api_method_name";
}