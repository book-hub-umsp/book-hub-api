using DomainKeyWord = BookHub.Models.Books.KeyWord;

namespace BookHub.Storage.PostgreSQL.Abstractions;

/// <summary>
/// Описывает конвертер ключевых слов.
/// </summary>
public interface IKeyWordsConverter
{
    public IReadOnlySet<DomainKeyWord> ConvertToDomain(string json);

    public string ConvertToStorage(IReadOnlySet<DomainKeyWord> keyWords);
}