using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents.Account;

/// <summary>
/// Обновление по пользователю.
/// </summary>
/// <typeparam name="TAttribute">
/// Тип атрибута, который необходимо обновить.
/// </typeparam>
public sealed class UserUpdated<TAttribute> : UserUpdatedBase
{
    public TAttribute Attribute { get; }

    public UserUpdated(Id<User> id, TAttribute attribute) : base(id)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        Attribute = attribute;
    }
}
