using Microsoft.Extensions.Options;

namespace BookHub.API.Metrics.Configurations;

public sealed class KestrelMetricServerConfigurationValidator :
    IValidateOptions<KestrelMetricServerConfiguration>
{
    public ValidateOptionsResult Validate(
        string? name,
        KestrelMetricServerConfiguration options)
    {
        if (string.IsNullOrWhiteSpace(options.Host))
        {
            return ValidateOptionsResult.Fail(
                "Host can't be null, empty or has only whitespaces.");
        }

        if (options.Port < 1)
        {
            return ValidateOptionsResult.Fail("Port should be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}