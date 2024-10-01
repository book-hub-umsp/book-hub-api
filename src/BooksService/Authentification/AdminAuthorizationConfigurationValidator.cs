using System.Diagnostics;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;

namespace BooksService.Authentification;

/// <summary>
/// Валидатор для <see cref="AuthorizationConfiguration"/>.
/// </summary>
public sealed partial class AdminAuthorizationConfigurationValidator :
    IValidateOptions<AdminAuthorizationConfiguration>
{
    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, AdminAuthorizationConfiguration options)
    {
        Debug.Assert(options is not null);

        if (!options.Admins.Any())
        {
            return ValidateOptionsResult.Fail("The admins list can not be empty.");
        }

        var errors = Enumerable.Empty<string>();

        foreach (var client in options.Admins)
        {
            if (string.IsNullOrWhiteSpace(client))
            {
                errors = errors.Append(
                    "The client e-mail string cannot be empty, " +
                    "or null, or has only whitespaces.");
            }
            else if (!_emailRegex.IsMatch(client))
            {
                errors = errors.Append($"The client e-mail '{client}' is not valid.");
            }
        }

        return errors.Any()
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }

    private static readonly Regex _emailRegex = _generatedEmailRegex();

    [GeneratedRegex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])", RegexOptions.Compiled)]
    private static partial Regex _generatedEmailRegex();
}