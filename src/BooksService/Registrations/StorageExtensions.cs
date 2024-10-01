using Storage.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using Storage.PostgreSQL.Abstractions;
using StackExchange.Redis;

namespace BooksService.Registrations;

public static class StorageExtensions
{
    public static IServiceCollection AddPostgresStorage(
        this IServiceCollection services, 
        IConfiguration configuration)
        => services
            .AddDbContext<BooksContext>((sp, dbOpt) =>
                dbOpt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
            .AddSingleton<IRepositoryContext, RepositoryContext>()
            // may be not needed at all
            .AddNpgsqlDataSource(configuration.GetConnectionString("DefaultConnection")!);

    public static IServiceCollection AddRedisCache(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(
                _ =>
                {
                    var connectionString = configuration.GetConnectionString("RedisConnection")!;

                    return ConnectionMultiplexer.Connect(connectionString);
                    /*
                     * sudo apt install redis-server
                     * sudo systemctl restart redis.service
                     * sudo systemctl status redis
                     * redis-cli
                     */
                });
}
