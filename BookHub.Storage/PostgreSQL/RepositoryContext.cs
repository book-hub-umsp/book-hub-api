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
    public DbSet<User> Users => _context.Users;

    public DbSet<Book> Books => _context.Books;

    public DbSet<BookGenre> Genres => _context.Genres;

    public RepositoryContext(BooksHubContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private readonly BooksHubContext _context;
}