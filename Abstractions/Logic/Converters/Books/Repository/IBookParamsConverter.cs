using ContractBookParams = BookHub.API.Contracts.REST.Requests.Books.Repository.BookParamsBase;
using DomainBookParams = BookHub.API.Models.CRUDS.Requests.BookParamsBase;

namespace BookHub.API.Abstractions.Logic.Converters.Books.Repository;

/// <summary>
/// Описывает конвертер параметров запроса к книгам.
/// </summary>
public interface IBookParamsConverter
{
    public DomainBookParams Convert(ContractBookParams contractParams);
}
