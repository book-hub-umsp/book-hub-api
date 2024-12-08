using BookHub.Models.Books.Repository;
using BookHub.Storage.PostgreSQL.Models.Helpers;

namespace BookHub.Storage.PostgreSQL.Abstractions.Converters;

/// <summary>
/// Описывает конвертера модели превью книги.
/// </summary>
public interface IBookPreviewConverter
{
    /// <summary>
    /// Конвертирует вспомогательную модель хранилища
    /// в доменную модель превью книги. 
    /// </summary>
    /// <param name="storageHelper">
    /// Вспомогательная модель хранилища.
    /// </param>
    /// <returns>
    /// <see cref="BookPreview"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="storageHelper"/> равен <see langword="null"/>.
    /// </exception>
    public BookPreview ToDomain(StorageBookPreviewHelper storageHelper);
}