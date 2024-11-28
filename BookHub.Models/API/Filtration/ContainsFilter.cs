using System;

namespace BookHub.Models.API.Filtration;

public sealed class ContainsFilter : FilterBase
{
    public object Value { get; }

    public ContainsFilter(
        string propertyName, 
        object value) : base(propertyName)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }
}
