using Microsoft.Extensions.Options;

namespace BookHub.Metrics.Configurations;

[OptionsValidator]
public sealed partial class TargetAppSourceMetricsConfigurationValidator :
    IValidateOptions<TargetAppSourceMetricsConfiguration>
{
}
