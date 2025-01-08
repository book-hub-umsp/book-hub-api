using BookHub.Metrics.Configurations;
using BookHub.Metrics.TargetFactory;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OpenTelemetry.Metrics;

namespace BookHub.Metrics;

/// <summary>
/// Сервер Kestrel, являющийся сайдкаром основного приложения 
/// для сбора и экспорта метрик в Prometheus.
/// </summary>
public sealed class KestrelMetricsBackgroundServer : BackgroundService
{
    public KestrelMetricsBackgroundServer(
        IOptions<KestrelMetricServerConfiguration> options,
        ILogger<KestrelMetricsBackgroundServer> logger,
        IMetricTargetFactory metricTargetFactory)
    {
        ArgumentNullException.ThrowIfNull(metricTargetFactory);
        _targets = metricTargetFactory.CreateTargets();

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var config = options?.Value 
            ?? throw new ArgumentNullException(nameof(options));

        _hostAddress = $"https://{config.Host}:{config.Port}";

        _logger.LogInformation(
            "Kestrel server can be accessed by host '{Host}'",
            _hostAddress);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
        await new WebHostBuilder()
            .UseKestrel()
            .ConfigureServices(
                services => services.AddOpenTelemetry()
                    .WithMetrics(providerBuilder =>
                    {
                        _ = providerBuilder.AddPrometheusExporter();
                        _ = providerBuilder.AddRuntimeInstrumentation();

                        foreach (var target in _targets)
                        {
                            _ = providerBuilder.AddMeter(target);

                            _ = providerBuilder.AddInstrumentation(() =>
                                new ActivityToMetricsInstrumentation(target));
                        }
                    }))
            .Configure(
                app => app.UseOpenTelemetryPrometheusScrapingEndpoint())
            .UseUrls(_hostAddress)
            .Build()
            .RunAsync(stoppingToken);

    private readonly string _hostAddress;
    private readonly IEnumerable<string> _targets;

    private readonly ILogger<KestrelMetricsBackgroundServer> _logger;
}