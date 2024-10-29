namespace BookHub.Contracts;

/// <summary>
/// Тип команды.
/// </summary>
public enum CommandType
{
    add_book,

    add_author_book,

    get_book,

    update_book
}