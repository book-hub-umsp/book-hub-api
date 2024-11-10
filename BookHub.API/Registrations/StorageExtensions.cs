using BookHub.Abstractions.Storage;
using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models.Books;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace BookHub.API.Registrations;

public static class StorageExtensions
{
    public static IServiceCollection AddPostgresStorage(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<BooksHubContext>((sp, dbOpt) =>
                dbOpt.UseNpgsql(CreateDataSource(configuration)))
            .AddScoped<IRepositoryContext, RepositoryContext>()
            .AddScoped<IBooksHubUnitOfWork, BooksHubUnitOfWork>()
            .AddScoped<IBooksRepository, BooksRepository>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IBooksGenresRepository, BooksGenresRepository>()

            .AddSingleton<IKeyWordsConverter, KeyWordsConverter>();

    private static NpgsqlDataSource CreateDataSource(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("DefaultConnection"));

        _ = dataSourceBuilder
            .MapEnum<BookStatus>()
            .MapEnum<UserStatus>()
            .MapEnum<UserRole>();

        return dataSourceBuilder.Build();
    }
}