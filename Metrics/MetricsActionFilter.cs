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
        ObjectDisposedException.ThrowIf(_disposed, this);

        var actionName = 
            $"{context.ActionDescriptor.RouteValues["controller"]}" +
            $".{context.ActionDescriptor.RouteValues["action"]}";

        _metricsManager.CountRequestsCalls(actionName);

        _metricJob = _metricsManager.MeasureRequestsExecution(actionName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _metricJob!.Dispose();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _metricJob?.Dispose();
            _disposed = true;
        }
    }

    private bool _disposed;

    private IDisposable? _metricJob;

    private readonly IMetricsManager _metricsManager;
}