using BookHub.Contracts.CRUDS;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookHub.Contracts;

/// <summary>
/// Конвертер для параметров команды.
/// </summary>
public sealed class CommandConverter : JsonConverter<BookParamsBase>
{
    public override BookParamsBase? ReadJson(
        JsonReader reader, 
        Type objectType, 
        BookParamsBase? existingValue, 
        bool hasExistingValue, 
        JsonSerializer serializer)
    {
        var tokenizedRawCommand = JObject.Load(reader);

        var type = tokenizedRawCommand["type"]!.ToObject<CommandType>();

        return type switch
        {
            CommandType.add_book => tokenizedRawCommand.ToObject<AddBookParams>(),

            CommandType.add_author_book => tokenizedRawCommand.ToObject<AddAuthorBookParams>(),

            CommandType.get_book => tokenizedRawCommand.ToObject<GetBookParams>(),

            CommandType.update_book => tokenizedRawCommand.ToObject<UpdateBookParams>(),

            _ => throw new InvalidOperationException($"Command type : '{type}' is not supported.")
        };
    }

    public override void WriteJson(
        JsonWriter writer, 
        BookParamsBase? value, 
        JsonSerializer serializer) 
        => throw new NotSupportedException("No need to support.");
}
