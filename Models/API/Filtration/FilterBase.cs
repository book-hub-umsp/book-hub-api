namespace BookHub.API.Models.API.Filtration;

public abstract class FilterBase
{
    public string PropertyName { get; }

    protected FilterBase(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        PropertyName = value;
    }
}
