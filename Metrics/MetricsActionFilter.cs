using BookHub.Abstractions.Metrics;

using Microsoft.AspNetCore.Mvc.Filters;

namespace BookHub.Metrics;

public sealed class MetricsActionFilter : IActionFilter
{
    public MetricsActionFilter(
        IMetricsManager metricsManager)
    {
        _metricsManager = metricsManager 
            ?? throw new ArgumentNullException(nameof(metricsManager));
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _actionName = 
            $"{context.ActionDescriptor.RouteValues["controller"]}" +
            $".{context.ActionDescriptor.RouteValues["action"]}";

        _metricJob = _metricsManager.MeasureRequestsExecution(_actionName);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _metricsManager.CountRequestsCalls(_actionName!);

        _metricJob!.Dispose();
    }

    private string? _actionName;

    private IDisposable? _metricJob;

    private readonly IMetricsManager _metricsManager;
}