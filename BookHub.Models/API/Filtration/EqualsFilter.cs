using System;

namespace BookHub.Models.API.Filtration;

public sealed class EqualsFilter : FilterBase
{
    public object Value { get; }

    public EqualsFilter(
        string propertyName, 
        object value) : base(propertyName)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }
}
