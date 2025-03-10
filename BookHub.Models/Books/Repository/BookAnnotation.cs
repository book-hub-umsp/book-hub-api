﻿using System;

namespace BookHub.Models.Books.Repository;

/// <summary>
/// Аннотация книги.
/// </summary>
public sealed record class BookAnnotation
{
    public string Content { get; }

    public BookAnnotation(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        content = content.Trim();

        if (content.Length > MAX_LENGHT)
        {
            throw new ArgumentException(
                $"Annotation max lenght must be {MAX_LENGHT} symbols");
        }

        if (content.Length < MIN_LENGHT)
        {
            throw new ArgumentException(
                $"Annotation min lenght must be {MIN_LENGHT} symbols");
        }

        Content = content;
    }

    private const int MIN_LENGHT = 50;
    private const int MAX_LENGHT = 1000;
}