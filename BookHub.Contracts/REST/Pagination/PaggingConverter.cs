using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookHub.Contracts.REST.Pagination;

public sealed class PaggingConverter : JsonConverter<PaggingBase>
{
    public override PaggingBase? ReadJson(JsonReader reader, Type objectType, PaggingBase? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var raw = JObject.Load(reader);

        if (raw["page_size"] is not null)
        {
            return raw.ToObject<PagePagging>();
        }
        else
        {
            return raw.ToObject<OffsetPagging>();
        }
    }

    public override void WriteJson(JsonWriter writer, PaggingBase? value, JsonSerializer serializer) =>
        throw new NotSupportedException();
}
