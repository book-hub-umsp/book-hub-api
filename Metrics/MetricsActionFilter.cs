using BookHub.Abstractions.Metrics;

using Microsoft.AspNetCore.Mvc.Filters;

namespace BookHub.Metrics;

public sealed class MetricsActionFilter : 
    IActionFilter,
    IDisposable
{
    public MetricsActionFilter(
        IMetricsManager metricsManager)
    {
        _metricsManager = metricsManager 
            ?? throw new ArgumentNullException(nameof(metricsManager));
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = 
            $"{context.ActionDescriptor.RouteValues["controller"]}" +
            $".{context.ActionDescriptor.RouteValues["action"]}";

        _metricsManager.CountRequestsCalls(actionName);

        ObjectDisposedException.ThrowIf(_isMetricJobDisposed, this);

        _metricJob = _metricsManager.MeasureRequestsExecution(actionName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        ObjectDisposedException.ThrowIf(_isMetricJobDisposed, this);

        _metricJob!.Dispose();

        _isMetricJobDisposed = true;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (!_isMetricJobDisposed)
            {
                _metricJob!.Dispose();
            }

            _disposed = true;
        }
    }

    private bool _disposed;

    private bool _isMetricJobDisposed;
    private IDisposable? _metricJob;

    private readonly IMetricsManager _metricsManager;
}