using BookHub.API.Abstractions.Metrics;
using BookHub.API.Metrics;
using BookHub.API.Metrics.Configurations;
using BookHub.API.Metrics.TargetFactory;
using BookHub.Metrics.Configurations;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace BookHub.API.Service.Registrations;

internal static class MetricsExtensions
{
    public static IServiceCollection AddMetrics(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddScoped<IActionFilter, MetricsActionFilter>()

            .AddControllers(opt => opt.Filters.Add<MetricsActionFilter>())
            .Services

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