﻿using BookHub.Abstractions.Storage;
using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models.Account;
using BookHub.Models.Books.Repository;
using BookHub.Storage.PostgreSQL;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace BookHub.API.Registrations;

static internal class StorageExtensions
{
    public static IServiceCollection AddPostgresStorage(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        IConfiguration configuration)
        => services
            .AddDbContext(environment, configuration)
            .AddScoped<IRepositoryContext, RepositoryContext>()
            .AddScoped<IBooksHubUnitOfWork, BooksHubUnitOfWork>()
            .AddScoped<IBooksRepository, BooksRepository>()
            .AddScoped<IChaptersRepository, ChaptersRepository>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IBooksGenresRepository, BooksGenresRepository>()
            .AddScoped<IFavoriteLinkRepository, FavoriteLinkRepository>()
            .AddScoped<IRolesRepository, RolesRepository>();

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("DefaultConnection"));

        _ = dataSourceBuilder
            .MapEnum<BookStatus>()
            .MapEnum<UserStatus>()
            .MapEnum<Permission>("permission_type");

        var source = dataSourceBuilder.Build();

        return services.AddDbContext<BooksHubContext>(opt => opt
            .UseNpgsql(source)
            .EnableSensitiveDataLogging(environment.IsDevelopment()));
    }
}