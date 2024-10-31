using Abstractions.Storage.Repositories;

using BookHub.Abstractions;
using BookHub.Models.Books;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace BooksService.Registrations;

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

            .AddSingleton<IKeyWordsConverter, KeyWordsConverter>();

    private static NpgsqlDataSource CreateDataSource(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("DefaultConnection"));

        _ = dataSourceBuilder
            .MapEnum<BookStatus>()
            .MapEnum<UserStatus>();

        return dataSourceBuilder.Build();
    }
}
