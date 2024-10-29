using ContractBookParams = BookHub.Contracts.CRUDS.Requests.BookParamsBase;
using DomainBookParams = BookHub.Models.CRUDS.Requests.BookParamsBase;

namespace Abstractions.Logic.Converters;

/// <summary>
/// Описывает конвертер параметров запроса к книгам.
/// </summary>
public interface IBookParamsConverter
{
    public DomainBookParams Convert(ContractBookParams contractParams);
}
