namespace BookHub.API.Contracts.REST;

public interface IRequestModel<TDomain>
{
    TDomain ToDomain();
}
