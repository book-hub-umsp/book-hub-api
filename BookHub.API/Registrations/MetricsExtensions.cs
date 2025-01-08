using BookHub.Abstractions.Metrics;
using BookHub.Metrics;
using BookHub.Metrics.Configurations;
using BookHub.Metrics.TargetFactory;

using Microsoft.Extensions.Options;

namespace BookHub.API.Registrations;

static internal class MetricsExtensions
{
    public static IServiceCollection AddMetrics(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddSingleton<IMetricsManager, MetricsManager>()
            .AddSingleton<IMetricTargetFactory, MetricTargetFactory>()
            .AddHostedService<KestrelMetricsBackgroundServer>()

            .Configure<KestrelMetricServerConfiguration>(
                configuration.GetSection(nameof(KestrelMetricServerConfiguration)))
            .AddSingleton<
                IValidateOptions<KestrelMetricServerConfiguration>,
                KestrelMetricServerConfigurationValidator>()

            .Configure<TargetAppSourceMetricsConfiguration>(
                configuration.GetSection(nameof(TargetAppSourceMetricsConfiguration)))
            .AddSingleton<
                IValidateOptions<TargetAppSourceMetricsConfiguration>,
                TargetAppSourceMetricsConfigurationValidator>();
}