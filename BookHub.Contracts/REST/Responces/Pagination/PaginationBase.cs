using BookHub.Models.API.Pagination;

namespace BookHub.Contracts.REST.Responces.Pagination;

public abstract class PaginationBase<TPagination>
    where TPagination : PaginationBase
{
    public abstract PaginationBase<TPagination> FromDomain(TPagination pagination);
}
