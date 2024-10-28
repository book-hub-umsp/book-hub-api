using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Конвертер ключевых слов.
/// </summary>
public sealed class KeyWordsConverter : IKeyWordsConverter
{
    public IReadOnlySet<KeyWord> ConvertToDomain(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        using var reader = new JsonTextReader(new StringReader(json));

        var keyWords = _serializer.Deserialize<IReadOnlySet<KeyWord>>(reader);

        Debug.Assert(keyWords != null);

        return keyWords;
    }

    public string ConvertToStorage(IReadOnlySet<KeyWord> keyWords)
    {
        ArgumentNullException.ThrowIfNull(keyWords);

        var stringBuilder = new StringBuilder();

        using var writer = new JsonTextWriter(new StringWriter(stringBuilder));

        _serializer.Serialize(writer, keyWords);

        return stringBuilder.ToString();
    }

    private static readonly JsonSerializer _serializer = JsonSerializer.CreateDefault();
}