using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents.Account;

public abstract class UserUpdatedBase : UpdatedBase
{
    public override Id<User> EntityId => (Id<User>)base.EntityId;

    protected UserUpdatedBase(Id<User> id) : base(id)
    {
    }
}
