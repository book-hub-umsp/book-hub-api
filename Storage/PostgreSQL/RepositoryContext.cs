﻿using BookHub.API.Storage.PostgreSQL.Abstractions;
using BookHub.API.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL;

/// <summary>
/// Контекст для репозиториев.
/// </summary>
public sealed class RepositoryContext : IRepositoryContext
{
    public DbSet<User> Users => _context.Users;

    public DbSet<Role> UserRoles => _context.UserRoles;

    public DbSet<Book> Books => _context.Books;

    public DbSet<Partition> Partitions => _context.Chapters;

    public DbSet<Keyword> Keywords => _context.Keywords;

    public DbSet<BookGenre> Genres => _context.Genres;

    public DbSet<FavoriteLink> FavoriteLinks => _context.FavoriteLinks;

    public DbSet<KeywordLink> KeywordLinks => _context.KeywordLinks;

    public RepositoryContext(BooksHubContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private readonly BooksHubContext _context;
}