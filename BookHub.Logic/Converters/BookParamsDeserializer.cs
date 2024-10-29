using System.Diagnostics;

using Abstractions.Logic.Converters;

using Newtonsoft.Json;

using DomainBookParams = BookHub.Models.CRUDS.BookParamsBase;
using ContractBookParams = BookHub.Contracts.CRUDS.BookParamsBase;
using BookHub.Contracts;

namespace BookHub.Logic.Converters;

/// <summary>
/// Десериализатор параметров запроса к книгам.
/// </summary>
public sealed class BookParamsDeserializer : IBookParamsDeserializer
{
    public BookParamsDeserializer(IBookParamsConverter converter)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
    }

    public DomainBookParams Deserialize(string source)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(source);

        using var reader = new JsonTextReader(new StringReader(source));

        var contract = _deserializer.Deserialize<ContractBookParams>(reader);

        Debug.Assert(contract != null);

        return _converter.Convert(contract);
    }

    private readonly IBookParamsConverter _converter;

    private static readonly JsonSerializer _deserializer =
        JsonSerializer.CreateDefault(
            new JsonSerializerSettings
            {
                Converters = [new CommandConverter()]
            });
}