using BookHub.API.Abstractions.Storage;
using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models.Account;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Storage.PostgreSQL;
using BookHub.API.Storage.PostgreSQL.Abstractions;
using BookHub.API.Storage.PostgreSQL.Repositories;
using BookHub.Storage.PostgreSQL.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace BookHub.API.Service.Registrations;

internal static class StorageExtensions
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