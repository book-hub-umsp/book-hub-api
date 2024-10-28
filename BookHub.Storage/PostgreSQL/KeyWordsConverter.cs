using BookHub.Contracts;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Collections.Immutable;

using DomainKeyWord = BookHub.Models.Books.KeyWord;
using ContractKeyWord = BookHub.Contracts.KeyWord;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Конвертер ключевых слов.
/// </summary>
public sealed class KeyWordsConverter : IKeyWordsConverter
{
    public IReadOnlySet<DomainKeyWord> ConvertToDomain(string? json)
    {
        // на случай дефолтного Null значения
        if (json is null)
        {
            return ImmutableHashSet<DomainKeyWord>.Empty;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        using var reader = new JsonTextReader(new StringReader(json));

        var keyWordsContainer = _serializer.Deserialize<KeyWordsContainer>(reader);

        Debug.Assert(keyWordsContainer != null);

        return keyWordsContainer.Keywords
            .Select(x => new DomainKeyWord(new(x.Content)))
            .ToHashSet();
    }

    public string ConvertToStorage(IReadOnlySet<DomainKeyWord> keyWords)
    {
        ArgumentNullException.ThrowIfNull(keyWords);

        if (!keyWords.Any())
        {
            throw new ArgumentException("Keywords list should not be empty.");
        }

        var container = new KeyWordsContainer
        {
            Keywords = keyWords
                .Select(x => new ContractKeyWord
                {
                    Content = x.Content.Value
                })
                .ToList()
        };

        var stringBuilder = new StringBuilder();

        using var writer = new JsonTextWriter(new StringWriter(stringBuilder));

        _serializer.Serialize(writer, container);

        return stringBuilder.ToString();
    }

    private static readonly JsonSerializer _serializer = JsonSerializer.CreateDefault();
}