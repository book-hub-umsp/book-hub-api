using BookHub.API.Abstractions;
using BookHub.API.Abstractions.Logic.Converters.Account;
using BookHub.API.Abstractions.Logic.Converters.Books.Repository;
using BookHub.API.Abstractions.Logic.Services.Account;
using BookHub.API.Abstractions.Logic.Services.Books.Content;
using BookHub.API.Abstractions.Logic.Services.Books.Repository;
using BookHub.API.Abstractions.Logic.Services.Favorite;
using BookHub.API.Logic.Converters.Account;
using BookHub.API.Logic.Converters.Books.Repository;
using BookHub.API.Logic.Services.Account;
using BookHub.API.Logic.Services.Books.Content;
using BookHub.API.Logic.Services.Books.Repository;
using BookHub.API.Logic.Services.Favorite;

using Microsoft.Extensions.Options;

namespace BookHub.API.Service.Registrations;

internal static class LogicExtensions
{
    public static IServiceCollection AddLogic(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddUserHandling()
            .AddBooksActionsHandling()
            .Configure<TestConfig>(configuration.GetRequiredSection(nameof(TestConfig)))
            .AddSingleton<IValidateOptions<TestConfig>, TestConfigValidator>();

    private static IServiceCollection AddUserHandling(
        this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddSingleton<IUserIdentityFacade, HttpUserIdentityFacade>()
            .AddSingleton<IUserRequestConverter, UserRequestConverter>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserFavoriteService, UserFavoriteService>()
            .AddScoped<IRolesService, RolesService>();

    private static IServiceCollection AddBooksActionsHandling(
        this IServiceCollection services)
        => services
            .AddScoped<IBookDescriptionService, BookDescriptionService>()
            .AddScoped<IBookGenresService, BookGenresService>()
            .AddScoped<IBookPartitionService, BookPartitionService>()
            .AddSingleton<IBookParamsConverter, BookParamsConverter>();
}