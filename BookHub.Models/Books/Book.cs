using System;
using System.ComponentModel;

namespace BookHub.Models.Books;

/// <summary>
/// Модель книги.
/// </summary>
public sealed class Book
{
    public BookAuthor Author { get; }

    public BookDefinition Definition 
    { 
        get { return Definition; } 

        private set { LastEditDate = DateTimeOffset.UtcNow; } 
    }

    public BookText Text
    {
        get { return Text; }

        private set { LastEditDate = DateTimeOffset.UtcNow; }
    }

    public BookStatus Status { get; private set; }

    public DateTimeOffset CreationDate { get; }

    public DateTimeOffset LastEditDate { get; private set; }

    public Book(
        BookAuthor author, 
        BookDefinition definition, 
        BookText text, 
        BookStatus status = BookStatus.Published)
    {
        Author = author ?? throw new ArgumentNullException(nameof(author));

        Definition = definition 
            ?? throw new ArgumentNullException(nameof(definition));

        Text = text ?? throw new ArgumentNullException(nameof(text));

        if (!Enum.IsDefined(status))
        {
            throw new InvalidEnumArgumentException(
                nameof(status),
                (int)status,
                typeof(BookStatus));
        }

        Status = status;

        CreationDate = DateTimeOffset.UtcNow;
        LastEditDate = CreationDate;
    }

    public void ChangeDefinition(BookDefinition newDefinition)
    {
        ArgumentNullException.ThrowIfNull(newDefinition);

        Definition = newDefinition;
    }

    public void ChangeText(BookText newText)
    {
        ArgumentNullException.ThrowIfNull(newText);

        Text = newText;
    }
}