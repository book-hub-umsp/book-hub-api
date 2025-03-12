using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents.Account;

public abstract class UserUpdatedBase : UpdatedBase
{
    public override Id<User> Id => (Id<User>)base.Id;

    protected UserUpdatedBase(Id<User> id) : base(id)
    {
    }
}
