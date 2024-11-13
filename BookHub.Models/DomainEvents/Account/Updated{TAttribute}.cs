using System;

using BookHub.Models.Account;

namespace BookHub.Models.DomainEvents.Account;

/// <summary>
/// Обновление по пользователю.
/// </summary>
/// <typeparam name="TAttribute">
/// Тип атрибута, который необходимо обновить.
/// </typeparam>
public sealed class Updated<TAttribute> : UpdatedBase<User>
{
    public TAttribute Attribute { get; }

    public Updated(Id<User> id, TAttribute attribute) : base(id)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        Attribute = attribute;
    }
}
