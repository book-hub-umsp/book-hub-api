using ContractBookParams = BookHub.Contracts.REST.Requests.Books.Repository.BookParamsBase;
using DomainBookParams = BookHub.Models.CRUDS.Requests.BookParamsBase;

namespace BookHub.Abstractions.Logic.Converters.Books.Repository;

/// <summary>
/// Описывает конвертер параметров запроса к книгам.
/// </summary>
public interface IBookParamsConverter
{
    public DomainBookParams Convert(ContractBookParams contractParams);
}
