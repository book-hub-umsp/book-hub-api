using BookHub.Abstractions.Metrics;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BookHub.Metrics;

public sealed class MetricsActionFilter : IActionFilter
{
    public MetricsActionFilter(
        IMetricsManager metricsManager,
        ILogger<MetricsActionFilter> logger)
    {
        _metricsManager = metricsManager 
            ?? throw new ArgumentNullException(nameof(metricsManager));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _actionName = 
            $"{context.ActionDescriptor.RouteValues["controller"]}" +
            $".{context.ActionDescriptor.RouteValues["action"]}";

        using (_metricsManager.MeasureRequestsExecution(_actionName))
        {
            // some code
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _metricsManager.CountRequestsCalls(_actionName);

        _logger.LogInformation("Action '{ActionName}' was completed", _actionName);
    }

    private string _actionName;

    private readonly IMetricsManager _metricsManager;
    private readonly ILogger<MetricsActionFilter> _logger;
}