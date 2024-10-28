﻿using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Контекст для репозиториев.
/// </summary>
public sealed class RepositoryContext : IRepositoryContext
{
    public DbSet<Book> Books => _context.Books;

    public RepositoryContext(BooksContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private readonly BooksContext _context;
}