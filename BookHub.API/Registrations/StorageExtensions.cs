using BookHub.Storage.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using BookHub.Storage.PostgreSQL.Abstractions;
using StackExchange.Redis;
using Npgsql;
using BookHub.Models.Books;

namespace BooksService.Registrations;

public static class StorageExtensions
{
    public static IServiceCollection AddPostgresStorage(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<BooksContext>((sp, dbOpt) =>
                dbOpt.UseNpgsql(CreateDataSource(configuration)))
            .AddSingleton<IRepositoryContext, RepositoryContext>();

    private static NpgsqlDataSource CreateDataSource(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("DefaultConnection"));

        _ = dataSourceBuilder
            .MapEnum<BookGenre>()
            .MapEnum<BookStatus>();

        return dataSourceBuilder.Build();
    }
}