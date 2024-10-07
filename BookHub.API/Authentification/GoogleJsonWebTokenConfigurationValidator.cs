using System.Diagnostics;

using Google.Apis.Auth;

using Microsoft.Extensions.Options;

namespace BookHub.API.Authentification;

/// <summary>
/// Представляет валидатора <see cref="GoogleJsonWebSignature.ValidationSettings"/>.
/// </summary>
public sealed class GoogleJsonWebTokenConfigurationValidator : 
    IValidateOptions<GoogleJsonWebSignature.ValidationSettings>
{
    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, GoogleJsonWebSignature.ValidationSettings options)
    {
        Debug.Assert(name is not null);
        Debug.Assert(options is not null);

        var issues = new List<string>();

        if (options.Audience is not null)
        {
            foreach (var (audience, index) in options.Audience.Select((audience, index) => (audience, index)))
            {
                if (string.IsNullOrWhiteSpace(audience))
                {
                    issues.Add(
                        $"The audience item at index {index} cannot be empty, " +
                        $"or null, or has only whitespaces.");
                }
            }
        }
        else
        {
            issues.Add("The audience collection cannot be empty.");
        }

        return issues.Any() ?
            ValidateOptionsResult.Fail(issues)
            : ValidateOptionsResult.Success;
    }
}