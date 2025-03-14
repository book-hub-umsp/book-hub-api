namespace BookHub.API.Contracts.REST;
public interface IResponseModel<TResponse, TDomain>
{
    static abstract TResponse FromDomain(TDomain domain);
}
