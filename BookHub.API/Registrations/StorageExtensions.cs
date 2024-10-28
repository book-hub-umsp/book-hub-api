using BookHub.Storage.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using BookHub.Storage.PostgreSQL.Abstractions;
using StackExchange.Redis;
using Npgsql;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Repositories;

namespace BooksService.Registrations;

public static class StorageExtensions
{
    public static IServiceCollection AddPostgresStorage(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<BooksContext>((sp, dbOpt) =>
                dbOpt.UseNpgsql(CreateDataSource(configuration)))
            .AddScoped<IRepositoryContext, RepositoryContext>()
            .AddScoped<IBooksUnitOfWork, BooksUnitOfWork>()
            .AddScoped<IBooksRepository, BooksRepository>()

            .AddSingleton<IKeyWordsConverter, KeyWordsConverter>();

    private static NpgsqlDataSource CreateDataSource(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("DefaultConnection"));

        _ = dataSourceBuilder
            .MapEnum<BookGenre>()
            .MapEnum<BookStatus>();

        return dataSourceBuilder.Build();
    }

    //public static IServiceCollection AddRedisCache(
    //    this IServiceCollection services,
    //    IConfiguration configuration)
    //    => services
    //        .AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(
    //            _ =>
    //            {
    //                var connectionString = configuration.GetConnectionString("RedisConnection")!;

    //                return ConnectionMultiplexer.Connect(connectionString);
    //                /*
    //                 * sudo apt install redis-server
    //                 * sudo systemctl restart redis.service
    //                 * sudo systemctl status redis
    //                 * redis-cli
    //                 */
    //            });
}
