namespace BookHub.Abstractions.Metrics;

/// <summary>
/// Описывает менеджера для работы с метриками приложения.
/// </summary>
public interface IMetricsManager
{
    IDisposable? MeasureRequestsExecution(string apiMethodName);

    void CountRequestsCalls(string apiMethodName);
}