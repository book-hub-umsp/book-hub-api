using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BookHub.API.Contracts.REST.Filtration;

/// <summary>
/// Тип фильтра.
/// </summary>
[JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
public enum FilterType
{
    Equals,

    Contains
}
