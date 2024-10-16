using Microsoft.Extensions.Options;

namespace BookHub.Logic.Configuration;

/// <summary>
/// Валидатор для конфига.
/// </summary>
[OptionsValidator]
public sealed partial class BookTextConfigurationValidator
    : IValidateOptions<BookTextConfiguration>
{
}