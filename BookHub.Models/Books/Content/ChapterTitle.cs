using System;
using System.Globalization;
using System.Text;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель для заголовка главы.
/// </summary>
public sealed record class ChapterTitle
{
    public string Value { get; }

    public ChapterTitle(ChapterNumber number)
    {
        ArgumentNullException.ThrowIfNull(number);

        Value = string.Format(
            CultureInfo.InvariantCulture,
            _chapterNameFormat,
            number.Value);
    }

    private static readonly CompositeFormat _chapterNameFormat =
        CompositeFormat.Parse("Chapter {0}");
}