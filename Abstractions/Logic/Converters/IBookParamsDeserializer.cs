using DomainBookParams = BookHub.Models.CRUDS.Requests.BookParamsBase;

namespace Abstractions.Logic.Converters;

/// <summary>
/// Описывает десериализатор параметров запроса к книгам.
/// </summary>
public interface IBookParamsDeserializer
{
    public DomainBookParams Deserialize(string source);
}