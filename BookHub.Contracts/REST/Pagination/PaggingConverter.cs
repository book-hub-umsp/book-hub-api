using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookHub.Contracts.REST.Pagination;

public sealed class PaggingConverter : JsonConverter<PaggingBase>
{
    public override PaggingBase? ReadJson(
        JsonReader reader, 
        Type objectType, 
        PaggingBase? existingValue, 
        bool hasExistingValue, 
        JsonSerializer serializer)
    {
        var raw = JObject.Load(reader);

        return raw["type"]!.ToObject<PaggingType>() switch
        {
            PaggingType.Page => raw.ToObject<PagePagging>(),
            PaggingType.Offset => raw.ToObject<OffsetPagging>(),

            _ => throw new InvalidOperationException("Unknown pagging type.")
        };
    }

    public override void WriteJson(JsonWriter writer, PaggingBase? value, JsonSerializer serializer) =>
        throw new NotSupportedException();
}
