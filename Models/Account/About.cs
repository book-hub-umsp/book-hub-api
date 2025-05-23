﻿namespace BookHub.API.Models.Account;

/// <summary>
/// Об авторе.
/// </summary>
public sealed class About
{
    public string Content { get; }

    public About(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        content = content.Trim();

        if (content.Length > MAX_LENGHT)
        {
            throw new ArgumentException($"Content max length is: {MAX_LENGHT} symbols.");
        }

        Content = content;
    }

    private const int MAX_LENGHT = 250;
}
